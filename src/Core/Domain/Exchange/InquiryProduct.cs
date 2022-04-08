namespace FSH.WebApi.Domain.Exchange;

public class InquiryProduct : AuditableEntity
{
    public string Name { get; private set; } = default!;
    public int Quantity { get; private set; }
    public DateOnly PreferredDeliveryDate { get; private set; }
    public Guid InquiryId { get; private set; }
    public ICollection<OfferProduct> OfferProducts { get; private set; } = new List<OfferProduct>();

    private InquiryProduct()
    {
        // Required by ORM
    }

    public InquiryProduct(Guid inquiryId, string name, int quantity, DateOnly preferredDeliveryDate)
    {
        InquiryId = inquiryId;
        Name = name;
        Quantity = quantity;
        PreferredDeliveryDate = preferredDeliveryDate;
    }
}