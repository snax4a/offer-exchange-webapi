using FSH.WebApi.Application.Exchange.Inquiries;

namespace FSH.WebApi.Application.Exchange.Offers;

public class OfferWithInquiryDto : OfferDto
{
    public InquiryDto Inquiry { get; set; } = default!;
}