using FSH.WebApi.Application.Exchange.Traders;

namespace FSH.WebApi.Application.Exchange.Inquiries.DTOs;

public class InquiryDetailsDto : InquiryWithCountsDto
{
    public IList<InquiryProductDetailsDto> Products { get; set; } = default!;
    public IList<TraderDto> Recipients { get; set; } = default!;
    public Guid CreatedBy { get; set; }
    public string Title { get; set; } = default!;
}