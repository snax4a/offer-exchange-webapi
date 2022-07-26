using FSH.WebApi.Domain.Billing;

namespace FSH.WebApi.Application.Exchange.Billing.Customers.Specifications;

public class CustomerByUserIdSpec : Specification<Customer>, ISingleResultSpecification
{
    public CustomerByUserIdSpec(Guid userId) => Query.Where(c => c.UserId == userId);
}