using FSH.WebApi.Application.Exchange.Billing.Stripe.DTOs;

namespace FSH.WebApi.Application.Exchange.Billing.Customers.DTOs;

public class CustomerBillingDetailsDto : CustomerDto
{
    public StripeSubscriptionDto? CurrentSubscription { get; set; }
}