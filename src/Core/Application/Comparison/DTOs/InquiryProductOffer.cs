namespace FSH.WebApi.Application.Comparison.DTOs;

public class InquiryProductOfferDto
{
    public string ProductName { get; set; } = default!;
    public DateTime PreferredDeliveryDate { get; set; }
    public Guid OfferProductId { get; set; }
    public string CurrencyCode { get; set; } = default!;
    public short? VatRate { get; set; }
    public int Quantity { get; set; }
    public long NetPrice { get; set; }
    public long NetValue { get; set; }
    public long GrossValue { get; set; }
    public DateTime DeliveryDate { get; set; }
    public bool IsReplacement { get; set; }
    public string? ReplacementName { get; set; }
    public string? Freebie { get; set; }
    public Guid TraderId { get; set; }
    public string TraderFullName { get; set; } = default!;
    public string TraderEmail { get; set; } = default!;
}