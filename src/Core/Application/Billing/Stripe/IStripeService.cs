using FSH.WebApi.Application.Exchange.Billing.Stripe.DTOs;
using Stripe; // TODO: we should remove Stripe dependency from this project and work with DTOs instead.

namespace FSH.WebApi.Application.Billing.Stripe;

public interface IStripeService : ITransientService
{
    Event ConstructEvent(string json, string signature);
    Task<StripeCustomerPortalSessionDto> CreateCustomerPortalSession(string customerId, CancellationToken ct = default);
    Task<StripeCheckoutSessionDto> CreateCheckoutSession(string customerId, string priceId, CancellationToken ct = default);
    Task<Customer> CreateCustomer(string email, string name, string userId, CancellationToken ct = default);
    Task CreateOrUpdateSubscription(Subscription subscription, CancellationToken ct = default);
    Task<Subscription> RetrieveSubscription(string subscriptionId, CancellationToken ct = default);
    Domain.Billing.BillingPlan? GetBillingPlanForStripeProduct(string productId);
}