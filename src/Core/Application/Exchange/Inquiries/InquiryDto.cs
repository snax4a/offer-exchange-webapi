namespace FSH.WebApi.Application.Exchange.Inquiries;

public class InquiryDto : IDto
{
    public Guid Id { get; set; }
    public int ReferenceNumber { get; set; }
    public string Name { get; set; } = default!;
    public DateTime CreatedOn { get; set; }
}