using FSH.WebApi.Application.Billing.Stripe;
using FSH.WebApi.Application.Common.Exceptions;
using FSH.WebApi.Application.Common.Persistence;
using FSH.WebApi.Application.Exchange.Billing.Stripe.DTOs;
using FSH.WebApi.Domain.Billing;
using Mapster;
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
    private readonly IRepositoryWithEvents<StripeSubscription> _subscriptionRepository;

    public StripeService(
        ILogger<StripeService> logger,
        IOptions<StripeSettings> settings,
        IRepositoryWithEvents<StripeSubscription> subscriptionRepository)
    {
        _logger = logger;
        _settings = settings.Value;
        _subscriptionRepository = subscriptionRepository;
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

    public async Task CreateOrUpdateSubscription(StripeRoot.Subscription subscriptionData, CancellationToken ct = default)
    {
        var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionData.Id, ct);

        if (subscription is null)
        {
            var newSubscription = new StripeSubscription(
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
                subscriptionData.Livemode);

            await _subscriptionRepository.AddAsync(newSubscription, ct);
        }
        else
        {
            subscription.Update(
                subscriptionData.Status,
                subscriptionData.CancelAtPeriodEnd,
                subscriptionData.CancelAt,
                subscriptionData.CanceledAt,
                subscriptionData.CurrentPeriodEnd,
                subscriptionData.CurrentPeriodStart,
                subscriptionData.StartDate,
                subscriptionData.EndedAt,
                subscriptionData.TrialStart,
                subscriptionData.TrialEnd);

            await _subscriptionRepository.UpdateAsync(subscription, ct);
        }
    }

    public async Task<StripeRoot.Subscription> RetrieveSubscription(string subscriptionId, CancellationToken ct = default)
    {
        var service = new StripeRoot.SubscriptionService(_stripeClient);
        var subscription = await service.GetAsync(subscriptionId, null, null, ct);

        _ = subscription ?? throw new NotFoundException($"Subscription: ${subscriptionId} not found.");

        return subscription;
    }

    public BillingPlan? GetBillingPlanForStripeProduct(string productId)
    {
        if (productId == _settings.BasicProductId) return BillingPlan.Basic;
        if (productId == _settings.StandardProductId) return BillingPlan.Standard;
        if (productId == _settings.EnterpriseProductId) return BillingPlan.Enterprise;
        return null;
    }
}