namespace FSH.WebApi.Application.Exchange.Inquiries;

public class InquiryProductDto : IDto
{
    public Guid Id { get; set; }
    public Guid InquiryId { get; private set; }
    public string Name { get; private set; } = default!;
    public int Quantity { get; private set; }
    public DateTime PreferredDeliveryDate { get; private set; }
}