using FSH.WebApi.Application.Exchange.Inquiries.DTOs;

namespace FSH.WebApi.Application.Comparison.DTOs;

public class ProductOfferListDto : IDto
{
    public InquiryProductDto InquiryProduct { get; set; } = default!;
    public IList<ProductOfferDto> ProductOffers { get; set; } = default!;
}