namespace FSH.WebApi.Application.Exchange.Inquiries;

public class InquiryProductDetailsDto : IDto
{
    public Guid Id { get; set; }
    public Guid InquiryId { get; set; }
    public string Name { get; set; } = default!;
    public int Quantity { get; set; }
    public DateTime PreferredDeliveryDate { get; set; }
}