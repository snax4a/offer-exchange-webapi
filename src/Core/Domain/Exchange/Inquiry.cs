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

    private Inquiry() // Required by ef
    {
    }

    public Inquiry(
        int referenceNumber,
        string name,
        string title,
        List<InquiryProduct> products,
        List<InquiryRecipient> inquiryRecipients)
    {
        if (referenceNumber <= 0) throw new ArgumentException("Must be a positive number", nameof(referenceNumber));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentNullException(nameof(title));
        if (products.Count == 0) throw new ArgumentException("Cannot be empty list", nameof(products));
        if (inquiryRecipients.Count == 0) throw new ArgumentException("Cannot be empty list", nameof(inquiryRecipients));

        ReferenceNumber = referenceNumber;
        Name = name;
        Title = title;
        Products = products;
        InquiryRecipients = inquiryRecipients;
    }
}