namespace FSH.WebApi.Domain.Exchange;

public class OfferProduct : BaseEntity, IAggregateRoot
{
    public string CurrencyCode { get; private set; }
    public ushort? VatRate { get; private set; }
    public uint Quantity { get; private set; }
    public ulong NetPrice { get; private set; }
    public ulong NetValue => Quantity * NetPrice;
    public ulong GrossValue => NetValue + ((VatRate ?? 0UL) * NetValue / 100UL);
    public DateOnly DeliveryDate { get; private set; }
    public bool IsReplacement { get; private set; }
    public string? ReplacementName { get; private set; }
    public string? Freebie { get; private set; }
    public Guid OfferId { get; private set; }
    public virtual Offer Offer { get; private set; } = default!;
    public Guid InquiryProductId { get; private set; }
    public virtual InquiryProduct InquiryProduct { get; private set; } = default!;
    public ICollection<OrderProduct> Orders { get; private set; } = new List<OrderProduct>();

    public OfferProduct(
        Guid offerId,
        Guid inquiryProductId,
        string currencyCode,
        ushort? vatRate,
        uint quantity,
        ulong netPrice,
        DateOnly deliveryDate,
        bool isReplacement,
        string? replacementName,
        string? freebie)
    {
        if (offerId == Guid.Empty) throw new ArgumentException("Must be a valid Guid", nameof(offerId));
        if (inquiryProductId == Guid.Empty) throw new ArgumentException("Must be a valid Guid", nameof(inquiryProductId));
        if (string.IsNullOrWhiteSpace(currencyCode)) throw new ArgumentException("Must be valid ISO 4217", nameof(currencyCode));
        if (netPrice <= 0) throw new ArgumentException("Must be a positive number", nameof(netPrice));
        if (quantity <= 0) throw new ArgumentException("Must be a positive number", nameof(quantity));
        if (isReplacement && string.IsNullOrWhiteSpace(replacementName)) throw new ArgumentNullException(nameof(replacementName));

        OfferId = offerId;
        InquiryProductId = inquiryProductId;
        CurrencyCode = currencyCode;
        VatRate = vatRate;
        Quantity = quantity;
        NetPrice = netPrice;
        DeliveryDate = deliveryDate;
        IsReplacement = isReplacement;
        ReplacementName = replacementName;
        Freebie = freebie;
    }
}