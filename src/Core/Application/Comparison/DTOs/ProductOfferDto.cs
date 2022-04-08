namespace FSH.WebApi.Application.Comparison.DTOs;

public class ProductOfferDto : IDto
{
    public Guid Id { get; private set; }
    public Guid OfferId { get; private set; }
    public string CurrencyCode { get; set; } = default!;
    public short? VatRate { get; set; }
    public int Quantity { get; set; }
    public long NetPrice { get; set; }
    public long NetValue { get; set; }
    public long GrossValue { get; set; }
    public DateOnly DeliveryDate { get; set; }
    public bool IsReplacement { get; set; }
    public string? ReplacementName { get; set; }
    public string? Freebie { get; set; }
}