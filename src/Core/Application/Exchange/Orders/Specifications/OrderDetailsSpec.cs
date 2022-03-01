namespace FSH.WebApi.Application.Exchange.Orders.Specifications;

public class OrderDetailsSpec : Specification<Order, OrderDetailsDto>, ISingleResultSpecification
{
    public OrderDetailsSpec(Guid id, Guid userId) =>
        Query
            .Where(o => o.Id == id && o.CreatedBy == userId)
            .Include(o => o.Products);
}
