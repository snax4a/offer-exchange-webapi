namespace FSH.WebApi.Domain.Exchange;

public class Order : AuditableEntity, IAggregateRoot
{
    public decimal NetValue { get; private set; }
    public decimal GrossValue { get; private set; }
    public string CurrencyCode { get; private set; } = default!;
    public DeliveryCost DeliveryCost { get; private set; } = default!;
    public Guid TraderId { get; private set; }
    public virtual Trader Trader { get; private set; } = default!;
    public OrderStatus Status { get; private set; }
    public ICollection<OrderProduct> Products { get; private set; } = new List<OrderProduct>();

    private Order() // Required by ef
    {
    }

    public Order(
        string currencyCode,
        Guid traderId,
        IList<OfferProduct> offerProducts)
    {
        if (string.IsNullOrWhiteSpace(currencyCode) || currencyCode.Length != 3)
            throw new ArgumentException("Must be valid ISO 4217", nameof(currencyCode));
        if (traderId == Guid.Empty) throw new ArgumentException("Must be a valid Guid", nameof(traderId));
        if (offerProducts.Count == 0) throw new ArgumentException("Cannot be empty list", nameof(offerProducts));

        NetValue = offerProducts.Sum(op => op.NetValue);
        GrossValue = offerProducts.Sum(op => op.GrossValue);
        CurrencyCode = currencyCode;
        Status = OrderStatus.Waiting;
        TraderId = traderId;

        foreach (OfferProduct product in offerProducts)
        {
            Products.Add(new OrderProduct(Id, product.Id));
        }
    }
}