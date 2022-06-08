using FSH.WebApi.Application.Exchange.Addresses.DTOs;
using FSH.WebApi.Application.Exchange.Offers.DTOs;

namespace FSH.WebApi.Application.Exchange.Orders.DTOs;

public class OrderDetailsDto : OrderDto
{
    public AddressDto? ShippingAddress { get; set; }
    public IList<OfferProductDto> Products { get; set; } = default!;
}