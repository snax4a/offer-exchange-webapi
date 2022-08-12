using FSH.WebApi.Core.Shared.Extensions;

namespace FSH.WebApi.Domain.Exchange;

public class OfferProduct : BaseEntity
{
    public string CurrencyCode { get; private set; } = default!;
    public short? VatRate { get; private set; }
    public int Quantity { get; private set; }
    public long NetPrice { get; private set; }
    public long NetValue { get; private set; }
    public long GrossValue { get; private set; }
    public DateOnly DeliveryDate { get; private set; }
    public bool IsReplacement { get; private set; }
    public string? ReplacementName { get; private set; }
    public string? Freebie { get; private set; }
    public Guid OfferId { get; private set; }
    public virtual Offer Offer { get; private set; } = default!;
    public Guid InquiryProductId { get; private set; }
    public virtual InquiryProduct InquiryProduct { get; private set; } = default!;
    public ICollection<OrderProduct> Orders { get; private set; } = new List<OrderProduct>();

    private OfferProduct()
    {
        // Required by ORM
    }

    public OfferProduct(
        Guid offerId,
        Guid inquiryProductId,
        string currencyCode,
        short? vatRate,
        int quantity,
        long netPrice,
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
        NetValue = quantity * netPrice;
        GrossValue = NetValue + CalculateVAT(NetValue, VatRate ?? 0);
        DeliveryDate = deliveryDate;
        IsReplacement = isReplacement;
        ReplacementName = replacementName?.StripHtml();
        Freebie = freebie?.StripHtml();
    }

    private long CalculateVAT(long value, short vatRate)
    {
        return (long)Math.Round(vatRate * value / 100m, 0, MidpointRounding.AwayFromZero);
    }
}