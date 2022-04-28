using MassTransit;

namespace FSH.WebApi.Domain.Exchange;

public class Order : AuditableEntity, IAggregateRoot
{
    public long NetValue { get; private set; }
    public long GrossValue { get; private set; }
    public string CurrencyCode { get; private set; } = default!;
    public Guid TraderId { get; private set; }
    public virtual Trader Trader { get; private set; } = default!;
    public OrderStatus Status { get; private set; }
    public ICollection<OrderProduct> Products { get; private set; } = new List<OrderProduct>();

    private Order()
    {
        // Required by ORM
    }

    public Order(Guid traderId, IList<OfferProduct> offerProducts)
    {
        if (traderId == Guid.Empty) throw new ArgumentException("Must be a valid Guid", nameof(traderId));
        if (offerProducts.Count == 0) throw new ArgumentException("Cannot be empty list", nameof(offerProducts));

        Id = NewId.Next().ToGuid();
        NetValue = offerProducts.Sum(op => op.NetValue);
        GrossValue = offerProducts.Sum(op => op.GrossValue);
        CurrencyCode = offerProducts[0].CurrencyCode;
        Status = OrderStatus.Waiting;
        TraderId = traderId;

        foreach (OfferProduct product in offerProducts)
        {
            Products.Add(new OrderProduct(Id, product.Id));
        }
    }
}