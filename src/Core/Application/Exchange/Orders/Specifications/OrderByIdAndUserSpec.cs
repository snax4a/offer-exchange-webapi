namespace FSH.WebApi.Application.Exchange.Orders.Specifications;

public class OrderByIdAndUserSpec : Specification<Order, OrderDto>, ISingleResultSpecification
{
    public OrderByIdAndUserSpec(Guid id, Guid userId) =>
        Query.Where(g => g.Id == id && g.CreatedBy == userId);
}