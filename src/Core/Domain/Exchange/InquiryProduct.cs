namespace FSH.WebApi.Domain.Exchange;

public class InquiryProduct : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = default!;
    public int Quantity { get; private set; }
    public DateTime PreferredDeliveryDate { get; private set; }
    public Guid InquiryId { get; private set; }
    public virtual Inquiry Inquiry { get; private set; } = default!;
    public ICollection<OfferProduct> OfferProducts { get; private set; } = new List<OfferProduct>();

    public InquiryProduct(Guid inquiryId, string name, int quantity, DateTime preferredDeliveryDate)
    {
        InquiryId = inquiryId;
        Name = name;
        Quantity = quantity;
        PreferredDeliveryDate = preferredDeliveryDate;
    }
}