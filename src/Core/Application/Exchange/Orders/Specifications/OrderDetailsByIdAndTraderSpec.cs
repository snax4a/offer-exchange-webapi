namespace FSH.WebApi.Application.Exchange.Orders.Specifications;

public class OrderDetailsByIdAndTraderSpec : Specification<Order>, ISingleResultSpecification
{
    public OrderDetailsByIdAndTraderSpec(Guid id, Guid traderId) =>
        Query
            .Where(o => o.Id == id && o.TraderId == traderId)
            .Include(o => o.Trader)
            .Include(o => o.ShippingAddress!)
                .ThenInclude(a => a.Country)
            .Include(o => o.Products)
                .ThenInclude(p => p.OfferProduct)
                    .ThenInclude(op => op.InquiryProduct);
}