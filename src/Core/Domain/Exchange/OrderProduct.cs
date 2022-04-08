namespace FSH.WebApi.Domain.Exchange;

public class OrderProduct
{
    public Guid OrderId { get; private set; }
    public virtual Order Order { get; private set; } = default!;
    public Guid OfferProductId { get; private set; }
    public virtual OfferProduct OfferProduct { get; private set; } = default!;

    public OrderProduct()
    {
        // Required by ORM
    }

    public OrderProduct(Guid orderId, Guid offerProductId)
    {
        if (orderId == Guid.Empty) throw new ArgumentException("Cannot be empty Guid", nameof(orderId));
        if (offerProductId == Guid.Empty) throw new ArgumentException("Cannot be empty Guid", nameof(offerProductId));

        OrderId = orderId;
        OfferProductId = offerProductId;
    }
}