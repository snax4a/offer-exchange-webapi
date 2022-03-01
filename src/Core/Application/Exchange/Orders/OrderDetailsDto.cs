using FSH.WebApi.Application.Exchange.Offers;

namespace FSH.WebApi.Application.Exchange.Orders;

public class OrderDetailsDto : OrderDto
{
    public IList<OfferProductDto> Products { get; set; } = default!;
}