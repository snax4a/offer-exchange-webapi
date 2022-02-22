namespace FSH.WebApi.Application.Exchange.Offers;

public class OfferProductDto : IDto
{
    public string CurrencyCode { get; set; } = default!;
    public decimal VatRate { get; set; }
    public decimal NetPrice { get; set; }
    public DateTime DeliveryDate { get; set; }
    public bool IsReplacement { get; set; }
    public string? ReplacementName { get; set; }
    public string? Freebie { get; set; }
    public Guid OfferId { get; set; }
    public Guid InquiryProductId { get; set; }
}