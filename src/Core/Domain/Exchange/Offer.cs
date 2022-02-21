namespace FSH.WebApi.Domain.Exchange;

public class Offer : AuditableEntity, IAggregateRoot
{
    // TODO: Add relation to user
    public string CurrencyCode { get; private set; } = default!;
    public decimal NetValue { get; private set; }
    public decimal GrossValue { get; private set; }
    public DateTime? ExpirationDate { get; private set; }
    public DeliveryCost DeliveryCost { get; private set; } = default!;
    public string? Freebie { get; private set; }
    public bool HasFreebies { get; private set; }
    public bool HasReplacements { get; private set; }
    public Guid InquiryId { get; private set; }
    public virtual Inquiry Inquiry { get; private set; } = default!;
    public Guid TraderId { get; private set; }
    public virtual Trader Trader { get; private set; } = default!;
    public ICollection<OfferProduct> OfferProducts { get; private set; } = new List<OfferProduct>();

    private Offer() // Required by ef
    {
    }

    public Offer(
        Guid inquiryId,
        Guid traderId,
        string currencyCode,
        DeliveryCost deliveryCost,
        string? offerFreebie,
        List<OfferProduct> offerProducts)
    {
        if (inquiryId == Guid.Empty) throw new ArgumentException("Must be a valid Guid", nameof(inquiryId));
        if (traderId == Guid.Empty) throw new ArgumentException("Must be a valid Guid", nameof(traderId));
        if (string.IsNullOrWhiteSpace(currencyCode)) throw new ArgumentException("Must be valid ISO 4217", nameof(currencyCode));
        if (offerProducts.Count == 0) throw new ArgumentException("Cannot be empty list", nameof(offerProducts));

        InquiryId = inquiryId;
        TraderId = traderId;
        CurrencyCode = currencyCode;
        NetValue = offerProducts.Sum(op => op.NetValue);
        GrossValue = offerProducts.Sum(op => op.GrossValue);
        DeliveryCost = deliveryCost;
        Freebie = offerFreebie;
        HasFreebies = offerProducts.Any(op => !string.IsNullOrWhiteSpace(op.Freebie));
        HasReplacements = offerProducts.Any(op => op.IsReplacement);
        OfferProducts = offerProducts;
    }
}