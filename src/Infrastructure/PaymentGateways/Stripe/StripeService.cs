using FSH.WebApi.Application.Billing.Stripe;
using FSH.WebApi.Application.Common.Exceptions;
using FSH.WebApi.Application.Common.Persistence;
using FSH.WebApi.Application.Exchange.Billing.Stripe.DTOs;
using FSH.WebApi.Domain.Billing;
using Mapster;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace FSH.WebApi.Infrastructure.PaymentGateways.Stripe;

public class StripeService : IStripeService
{
    private readonly StripeSettings _settings;
    private readonly IStripeClient _stripeClient;
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
        _stripeClient = new StripeClient(_settings.SecretKey);
    }

    public Event ConstructEvent(string json, string signature)
    {
        return EventUtility.ConstructEvent(json, signature, _settings.WebhookSecret);
    }

    public async Task<StripeCheckoutSessionDto> CreateCheckoutSession(string customerId, string priceId, CancellationToken ct = default)
    {
        var options = new SessionCreateOptions
        {
            Mode = "subscription",
            Customer = customerId,
            CustomerUpdate = new SessionCustomerUpdateOptions
            {
                Name = "auto",
                Address = "auto"
            },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    Price = priceId,
                    Quantity = 1,
                },
            },
            SuccessUrl = $"{_settings.SuccessUrl}?session_id={{CHECKOUT_SESSION_ID}}",
            CancelUrl = $"{_settings.CancelUrl}",
            AutomaticTax = new SessionAutomaticTaxOptions { Enabled = true },
            TaxIdCollection = new SessionTaxIdCollectionOptions { Enabled = true }
        };

        var service = new SessionService(_stripeClient);
        var session = await service.CreateAsync(options, null, ct);

        _logger.LogInformation("Stripe checkout session created with id: {SessionId}", session.Id);

        return session.Adapt<StripeCheckoutSessionDto>();
    }

    public async Task CreateOrUpdateSubscription(Subscription subscriptionData, CancellationToken ct = default)
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

    public async Task<Subscription> RetrieveSubscription(string subscriptionId, CancellationToken ct = default)
    {
        var service = new SubscriptionService(_stripeClient);
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