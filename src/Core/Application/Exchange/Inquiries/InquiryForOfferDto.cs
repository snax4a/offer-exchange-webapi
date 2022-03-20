using FSH.WebApi.Application.Exchange.Traders;

namespace FSH.WebApi.Application.Exchange.Inquiries;

public class InquiryForOfferDto : IDto
{
    public Guid Id { get; set; }
    public int ReferenceNumber { get; set; }
    public string Title { get; set; } = default!;
    public DateTime CreatedOn { get; set; }
    public TraderDto Trader { get; set; } = default!;
    public IList<InquiryProductDetailsDto> Products { get; set; } = default!;
}