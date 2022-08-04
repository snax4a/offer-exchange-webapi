namespace FSH.WebApi.Domain.Billing;

public class StripeProduct : BaseEntity<string>, IAggregateRoot
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }
    public bool Livemode { get; private set; }
    public string Metadata { get; private set; }
    public ICollection<StripePrice> Prices { get; private set; }

    public StripeProduct(
        string id,
        string name,
        string? description,
        bool isActive,
        bool livemode,
        string metadata)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (string.IsNullOrWhiteSpace(metadata)) throw new ArgumentNullException(nameof(metadata));

        Id = id;
        Name = name;
        Description = description;
        IsActive = isActive;
        Livemode = livemode;
        Metadata = metadata;
        Prices = new List<StripePrice>();
    }

    public StripeProduct Update(
        string name,
        string? description,
        bool isActive,
        bool livemode,
        string metadata)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (string.IsNullOrWhiteSpace(metadata)) throw new ArgumentNullException(nameof(metadata));

        Name = name;
        Description = description;
        IsActive = isActive;
        Livemode = livemode;
        Metadata = metadata;

        return this;
    }
}