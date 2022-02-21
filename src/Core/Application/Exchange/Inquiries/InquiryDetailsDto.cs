using FSH.WebApi.Application.Exchange.Traders;

namespace FSH.WebApi.Application.Exchange.Inquiries;

public class InquiryDetailsDto : IDto
{
    public Guid Id { get; set; }
    public int ReferenceNumber { get; set; }
    public string Name { get; set; } = default!;
    public string Title { get; set; } = default!;
    public DateTime CreatedOn { get; set; }
    public int RecipientCount { get; set; }
    public int OfferCount { get; set; }
    public IList<InquiryProductDto> Products { get; set; } = default!;
    public IList<TraderDto> Recipients { get; set; } = default!;
}