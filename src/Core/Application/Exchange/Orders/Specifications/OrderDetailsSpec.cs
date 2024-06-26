using FSH.WebApi.Application.Exchange.Orders.DTOs;

namespace FSH.WebApi.Application.Exchange.Orders.Specifications;

public class OrderDetailsSpec : Specification<Order, OrderDetailsDto>, ISingleResultSpecification
{
    public OrderDetailsSpec(Guid id, Guid userId) =>
        Query
            .Where(o => o.Id == id && o.CreatedBy == userId)
            .Include(o => o.Trader)
            .Include(o => o.ShippingAddress!)
                .ThenInclude(a => a.Country)
            .Include(o => o.Products)
                .ThenInclude(p => p.OfferProduct)
                    .ThenInclude(op => op.InquiryProduct);
}
