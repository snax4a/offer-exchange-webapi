using FSH.WebApi.Domain.Billing;

namespace FSH.WebApi.Application.Exchange.Billing.Customers.Specifications;

public class CustomerWithSubscriptionByUserIdSpec : Specification<Customer>, ISingleResultSpecification
{
    public CustomerWithSubscriptionByUserIdSpec(Guid userId)
        => Query
            .Where(c => c.UserId == userId)
            .Include(c => c.CurrentSubscription);
}