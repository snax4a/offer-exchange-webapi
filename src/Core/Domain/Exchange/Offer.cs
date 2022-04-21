namespace FSH.WebApi.Domain.Exchange;

public class Offer : BaseEntity, IAggregateRoot, ICreatedOnInformation
{
    public string CurrencyCode { get; private set; } = default!;
    public long NetValue { get; private set; }
    public long GrossValue { get; private set; }
    public DateOnly? ExpirationDate { get; private set; }
    public DeliveryCost DeliveryCost { get; private set; } = default!;
    public string? Freebie { get; private set; }
    public bool HasFreebies { get; private set; }
    public bool HasReplacements { get; private set; }
    public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;
    public Guid UserId { get; private set; }
    public Guid InquiryId { get; private set; }
    public virtual Inquiry Inquiry { get; private set; } = default!;
    public Guid TraderId { get; private set; }
    public virtual Trader Trader { get; private set; } = default!;
    public ICollection<OfferProduct> OfferProducts { get; private set; } = new List<OfferProduct>();

    private Offer()
    {
        // Required by ORM
    }

    public Offer(
        Guid id,
        Guid inquiryId,
        Guid traderId,
        Guid userId,
        DateOnly? expirationDate,
        string currencyCode,
        DeliveryCost deliveryCost,
        string? offerFreebie,
        IList<OfferProduct> offerProducts)
    {
        if (id == Guid.Empty) throw new ArgumentException("Cannot be empty Guid", nameof(id));
        if (inquiryId == Guid.Empty) throw new ArgumentException("Must be a valid Guid", nameof(inquiryId));
        if (traderId == Guid.Empty) throw new ArgumentException("Must be a valid Guid", nameof(traderId));
        if (userId == Guid.Empty) throw new ArgumentException("Must be a valid Guid", nameof(userId));
        if (offerProducts.Count == 0) throw new ArgumentException("Cannot be empty list", nameof(offerProducts));
        if (string.IsNullOrWhiteSpace(currencyCode) || currencyCode.Length != 3)
            throw new ArgumentException("Must be valid ISO 4217", nameof(currencyCode));

        Id = id;
        InquiryId = inquiryId;
        TraderId = traderId;
        UserId = userId;
        ExpirationDate = expirationDate;
        CurrencyCode = currencyCode;
        NetValue = offerProducts.Sum(op => op.NetValue);
        GrossValue = offerProducts.Sum(op => op.GrossValue);
        DeliveryCost = deliveryCost;
        Freebie = offerFreebie;
        HasFreebies = !string.IsNullOrWhiteSpace(offerFreebie) || offerProducts.Any(op => !string.IsNullOrWhiteSpace(op.Freebie));
        HasReplacements = offerProducts.Any(op => op.IsReplacement);
        OfferProducts = offerProducts;
    }
}