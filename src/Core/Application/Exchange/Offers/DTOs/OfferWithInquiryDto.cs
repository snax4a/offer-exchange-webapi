using FSH.WebApi.Application.Exchange.Inquiries.DTOs;

namespace FSH.WebApi.Application.Exchange.Offers.DTOs;

public class OfferWithInquiryDto : OfferDto
{
    public InquiryDto Inquiry { get; set; } = default!;
}