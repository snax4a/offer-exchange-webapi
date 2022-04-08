namespace FSH.WebApi.Domain.Exchange;

public class Inquiry : AuditableEntity, IAggregateRoot
{
    public int ReferenceNumber { get; private set; }
    public string Name { get; private set; } = default!;
    public string Title { get; private set; } = default!;
    public ICollection<InquiryProduct> Products { get; private set; } = new List<InquiryProduct>();
    public ICollection<InquiryRecipient> InquiryRecipients { get; private set; } = new List<InquiryRecipient>();
    public ICollection<Offer> Offers { get; private set; } = new List<Offer>();

    public int OfferCount => Offers.Count;
    public int RecipientCount => InquiryRecipients.Count;

    private Inquiry()
    {
        // Required by ORM
    }

    public Inquiry(
        Guid id,
        int referenceNumber,
        string name,
        string title,
        IList<InquiryProduct> products,
        IList<Guid> recipientIds)
    {
        if (id == Guid.Empty) throw new ArgumentException("Cannot be empty Guid", nameof(id));
        if (referenceNumber <= 0) throw new ArgumentException("Must be a positive number", nameof(referenceNumber));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentNullException(nameof(title));
        if (products.Count == 0) throw new ArgumentException("Cannot be empty list", nameof(products));
        if (recipientIds.Count == 0) throw new ArgumentException("Cannot be empty list", nameof(recipientIds));

        Id = id;
        ReferenceNumber = referenceNumber;
        Name = name;
        Title = title;
        Products = products;

        foreach (Guid traderId in recipientIds)
        {
            InquiryRecipients.Add(new InquiryRecipient(id, traderId));
        }
    }
}