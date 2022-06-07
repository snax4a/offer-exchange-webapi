using FSH.WebApi.Application.Exchange.Addresses.DTOs;
using FSH.WebApi.Application.Exchange.Offers.DTOs;
using FSH.WebApi.Application.Exchange.Traders.DTOs;
using FSH.WebApi.Application.Identity.Users;

namespace FSH.WebApi.Application.Exchange.Inquiries.DTOs;

public class InquiryForOfferDto : IDto
{
    public Guid Id { get; set; }
    public int ReferenceNumber { get; set; }
    public string Title { get; set; } = default!;
    public AddressDto? ShippingAddress { get; set; }
    public DateTime CreatedOn { get; set; }
    public TraderDto Trader { get; set; } = default!;
    public UserDto Creator { get; set; } = default!;
    public IList<InquiryProductDetailsDto> Products { get; set; } = default!;
    public OfferDetailsDto? Offer { get; set; }
}