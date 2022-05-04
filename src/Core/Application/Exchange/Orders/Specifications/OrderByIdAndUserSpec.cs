using FSH.WebApi.Application.Exchange.Orders.DTOs;

namespace FSH.WebApi.Application.Exchange.Orders.Specifications;

public class OrderByIdAndUserSpec : Specification<Order, OrderDto>, ISingleResultSpecification
{
    public OrderByIdAndUserSpec(Guid id, Guid userId) =>
        Query
            .Where(o => o.Id == id && o.CreatedBy == userId)
            .Include(o => o.Trader);
}