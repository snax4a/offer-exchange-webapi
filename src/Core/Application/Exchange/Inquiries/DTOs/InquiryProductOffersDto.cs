namespace FSH.WebApi.Application.Exchange.Inquiries.DTOs;

public class InquiryProductOffersDto : IDto
{
    public InquiryProductDto InquiryProduct { get; set; } = default!;
    public IList<OfferProductDto> OfferProducts { get; set; } = default!;
}