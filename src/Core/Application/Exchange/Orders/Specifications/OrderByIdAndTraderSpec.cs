namespace FSH.WebApi.Application.Exchange.Orders.Specifications;

public class OrderByIdAndTraderSpec : Specification<Order>, ISingleResultSpecification
{
    public OrderByIdAndTraderSpec(Guid id, Guid traderId) =>
        Query
            .Where(o => o.Id == id && o.TraderId == traderId);
}