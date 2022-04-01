namespace FSH.WebApi.Application.Exchange.Inquiries.DTOs;

public class InquiryWithCountsDto : InquiryDto
{
    public int RecipientCount { get; set; }
    public int OfferCount { get; set; }
}