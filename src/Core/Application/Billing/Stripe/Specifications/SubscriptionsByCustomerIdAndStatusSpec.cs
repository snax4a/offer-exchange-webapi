using FSH.WebApi.Domain.Billing;

namespace FSH.WebApi.Application.Exchange.Billing.Stripe.Specifications;

public class SubscriptionsByCustomerIdAndStatusSpec : Specification<StripeSubscription>
{
    public SubscriptionsByCustomerIdAndStatusSpec(string customerId, string status)
        => Query.Where(s => s.CustomerId == customerId && s.Status == status);
}