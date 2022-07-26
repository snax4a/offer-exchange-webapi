using FSH.WebApi.Domain.Billing;

namespace FSH.WebApi.Application.Exchange.Billing.Customers.Specifications;

public class CustomerByStripeCustomerIdSpec : Specification<Customer>, ISingleResultSpecification
{
    public CustomerByStripeCustomerIdSpec(string stripeCustomerId) =>
        Query.Where(c => c.StripeCustomerId == stripeCustomerId);
}