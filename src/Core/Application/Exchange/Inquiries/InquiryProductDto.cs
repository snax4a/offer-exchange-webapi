namespace FSH.WebApi.Application.Exchange.Inquiries;

public class InquiryProductDto : IDto
{
    public string Name { get; set; } = default!;
    public int Quantity { get; set; }
    public DateOnly PreferredDeliveryDate { get; set; }
}