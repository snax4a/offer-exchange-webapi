using FSH.WebApi.Application.Exchange.Billing.Stripe.DTOs;
using FSH.WebApi.Domain.Billing;
using Stripe; // TODO: we should remove Stripe dependency from this project and work with DTOs instead.

namespace FSH.WebApi.Application.Billing.Stripe;

public interface IStripeService : ITransientService
{
    Event ConstructEvent(string json, string signature);
    Task<StripeCheckoutSessionDto> CreateCheckoutSession(string customerId, string priceId, CancellationToken ct = default);
    Task CreateOrUpdateSubscription(Subscription subscription, CancellationToken ct = default);
    Task<Subscription> RetrieveSubscription(string subscriptionId, CancellationToken ct = default);
    BillingPlan? GetBillingPlanForStripeProduct(string productId);
}