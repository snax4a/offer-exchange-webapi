using System.Text.Json;
using FSH.WebApi.Application.Billing.Stripe;
using FSH.WebApi.Application.Common.Exceptions;
using FSH.WebApi.Application.Common.Persistence;
using FSH.WebApi.Application.Exchange.Billing.Stripe.DTOs;
using FSH.WebApi.Domain.Billing;
using FSH.WebApi.Infrastructure.Persistence.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// Doing this to avoid namespace conflicts.
using StripeBilling = Stripe.BillingPortal;
using StripeCheckout = Stripe.Checkout;
using StripeRoot = Stripe;

namespace FSH.WebApi.Infrastructure.PaymentGateways.Stripe;

public class StripeService : IStripeService
{
    private readonly StripeSettings _settings;
    private readonly StripeRoot.IStripeClient _stripeClient;
    private readonly ILogger<StripeService> _logger;
    private readonly ApplicationDbContext _db;

    public StripeService(
        ILogger<StripeService> logger,
        IOptions<StripeSettings> settings,
        ApplicationDbContext dbContext)
    {
        _db = dbContext;
        _logger = logger;
        _settings = settings.Value;
        _stripeClient = new StripeRoot.StripeClient(_settings.SecretKey);
    }

    public StripeRoot.Event ConstructEvent(string json, string signature)
    {
        return StripeRoot.EventUtility.ConstructEvent(json, signature, _settings.WebhookSecret);
    }

    public async Task<StripeCustomerPortalSessionDto> CreateCustomerPortalSession(string customerId, CancellationToken ct = default)
    {
        var options = new StripeBilling.SessionCreateOptions
        {
            Customer = customerId,
            ReturnUrl = _settings.PortalReturnUrl,
        };

        var service = new StripeBilling.SessionService(_stripeClient);
        var session = await service.CreateAsync(options, null, ct);

        _logger.LogInformation("Stripe checkout session created with id: {SessionId}", session.Id);

        return session.Adapt<StripeCustomerPortalSessionDto>();
    }

    public async Task<StripeCheckoutSessionDto> CreateCheckoutSession(string customerId, string priceId, CancellationToken ct = default)
    {
        var options = new StripeCheckout.SessionCreateOptions
        {
            Mode = "subscription",
            Customer = customerId,
            CustomerUpdate = new StripeCheckout.SessionCustomerUpdateOptions
            {
                Name = "auto",
                Address = "auto"
            },
            LineItems = new List<StripeCheckout.SessionLineItemOptions>
            {
                new StripeCheckout.SessionLineItemOptions
                {
                    Price = priceId,
                    Quantity = 1,
                },
            },
            SuccessUrl = $"{_settings.SuccessUrl}?session_id={{CHECKOUT_SESSION_ID}}",
            CancelUrl = $"{_settings.CancelUrl}",
            AutomaticTax = new StripeCheckout.SessionAutomaticTaxOptions { Enabled = true },
            TaxIdCollection = new StripeCheckout.SessionTaxIdCollectionOptions { Enabled = true }
        };

        var service = new StripeCheckout.SessionService(_stripeClient);
        var session = await service.CreateAsync(options, null, ct);

        _logger.LogInformation("Stripe checkout session created with id: {SessionId}", session.Id);

        return session.Adapt<StripeCheckoutSessionDto>();
    }

    public async Task<StripeRoot.Customer> CreateCustomer(string email, string name, string userId, CancellationToken ct = default)
    {
        var options = new StripeRoot.CustomerCreateOptions
        {
            Email = email,
            Name = name,
            Metadata = new Dictionary<string, string>
            {
                { "userId", userId }
            }
        };
        var service = new StripeRoot.CustomerService(_stripeClient);
        return await service.CreateAsync(options, null, ct);
    }

    public async Task<StripeRoot.Customer> UpdateCustomer(string customerId, string email, string name, CancellationToken ct = default)
    {
        var options = new StripeRoot.CustomerUpdateOptions
        {
            Email = email,
            Name = name,
        };
        var service = new StripeRoot.CustomerService(_stripeClient);
        return await service.UpdateAsync(customerId, options, null, ct);
    }

    public async Task UpsertSubscription(StripeRoot.Subscription subscriptionData, CancellationToken ct = default)
    {
        await _db.StripeSubscriptions
            .Upsert(new StripeSubscription(
                subscriptionData.Id,
                subscriptionData.CustomerId,
                subscriptionData.Status,
                subscriptionData.Items.Data[0].Price.Id,
                subscriptionData.CancelAtPeriodEnd,
                subscriptionData.CancelAt,
                subscriptionData.CanceledAt,
                subscriptionData.CollectionMethod,
                subscriptionData.Created,
                subscriptionData.Items.Data[0].Price.Currency,
                subscriptionData.CurrentPeriodEnd,
                subscriptionData.CurrentPeriodStart,
                subscriptionData.StartDate,
                subscriptionData.EndedAt,
                subscriptionData.TrialStart,
                subscriptionData.TrialEnd,
                subscriptionData.Livemode))
            .RunAsync(ct);
    }

    public async Task<StripeRoot.Subscription> RetrieveSubscription(string subscriptionId, CancellationToken ct = default)
    {
        var service = new StripeRoot.SubscriptionService(_stripeClient);
        var subscription = await service.GetAsync(subscriptionId, null, null, ct);

        _ = subscription ?? throw new NotFoundException($"Subscription: ${subscriptionId} not found.");

        return subscription;
    }

    public async Task UpsertProduct(StripeRoot.Product productData, CancellationToken ct = default)
    {
        string serializedMetadata = JsonSerializer.Serialize(productData.Metadata);

        await _db.StripeProducts
            .Upsert(new StripeProduct(
                productData.Id,
                productData.Name,
                productData.Description,
                productData.Active,
                productData.Livemode,
                serializedMetadata))
            .RunAsync(ct);
    }

    public async Task<IEnumerable<StripeRoot.Product>> ListAllActiveProducts(CancellationToken ct = default)
    {
        var service = new StripeRoot.ProductService(_stripeClient);
        var options = new StripeRoot.ProductListOptions { Active = true, Limit = 100 };
        var products = await service.ListAsync(options, null, ct);

        return products.Data;
    }

    public async Task<IEnumerable<StripeRoot.Price>> ListAllActivePrices(CancellationToken ct = default)
    {
        var service = new StripeRoot.PriceService(_stripeClient);
        var options = new StripeRoot.PriceListOptions { Active = true, Limit = 100 };
        var prices = await service.ListAsync(options, null, ct);

        return prices.Data;
    }

    public async Task UpsertPrice(StripeRoot.Price priceData, CancellationToken ct = default)
    {
        // var price = await _priceRepository.GetByIdAsync(priceData.Id, ct);
        string serializedMetadata = JsonSerializer.Serialize(priceData.Metadata);

        await _db.StripePrices
            .Upsert(new StripePrice(
                priceData.Id,
                priceData.ProductId,
                priceData.Type,
                priceData.Nickname,
                priceData.UnitAmount,
                priceData.UnitAmountDecimal,
                priceData.Currency,
                priceData.TaxBehavior,
                priceData.Recurring.Interval,
                priceData.Recurring.IntervalCount,
                priceData.Recurring.TrialPeriodDays,
                priceData.Active,
                priceData.Livemode,
                serializedMetadata))
            .RunAsync(ct);
    }

    public BillingPlan? GetBillingPlanForStripeProduct(string productId)
    {
        if (productId == _settings.BasicProductId) return BillingPlan.Basic;
        if (productId == _settings.StandardProductId) return BillingPlan.Standard;
        if (productId == _settings.EnterpriseProductId) return BillingPlan.Enterprise;
        return null;
    }
}