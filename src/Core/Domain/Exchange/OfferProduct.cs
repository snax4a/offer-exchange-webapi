namespace FSH.WebApi.Domain.Exchange;

public class OfferProduct : AuditableEntity, IAggregateRoot
{
    public string CurrencyCode { get; private set; }
    public decimal VatRate { get; private set; }
    public decimal NetPrice { get; private set; }
    public decimal NetValue => InquiryProduct.Quantity * NetPrice;
    public decimal GrossValue => InquiryProduct.Quantity * NetPrice * (1 + VatRate);
    public DateTime DeliveryDate { get; private set; }
    public bool IsReplacement { get; private set; }
    public string? ReplacementName { get; private set; }
    public string? Freebie { get; private set; }
    public Guid OfferId { get; private set; }
    public virtual Offer Offer { get; private set; } = default!;
    public Guid InquiryProductId { get; private set; }
    public virtual InquiryProduct InquiryProduct { get; private set; } = default!;

    public OfferProduct(
        Guid offerId,
        Guid inquiryProductId,
        string currencyCode,
        decimal vatRate,
        decimal netPrice,
        DateTime deliveryDate,
        bool isReplacement,
        string replacementName,
        string freebie)
    {
        if (offerId == Guid.Empty) throw new ArgumentException("Must be a valid Guid", nameof(offerId));
        if (inquiryProductId == Guid.Empty) throw new ArgumentException("Must be a valid Guid", nameof(inquiryProductId));
        if (string.IsNullOrWhiteSpace(currencyCode)) throw new ArgumentException("Must be valid ISO 4217", nameof(currencyCode));
        if (netPrice <= 0) throw new ArgumentException("Must be a positive number", nameof(netPrice));
        if (deliveryDate <= DateTime.UtcNow) throw new ArgumentException("Must be a future date", nameof(deliveryDate));
        if (isReplacement && string.IsNullOrWhiteSpace(replacementName)) throw new ArgumentNullException(nameof(replacementName));

        OfferId = offerId;
        InquiryProductId = inquiryProductId;
        CurrencyCode = currencyCode;
        VatRate = vatRate;
        NetPrice = netPrice;
        DeliveryDate = deliveryDate;
        IsReplacement = isReplacement;
        ReplacementName = replacementName;
        Freebie = freebie;
    }
}