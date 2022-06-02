namespace FSH.WebApi.Domain.Exchange;

public class UserAddress : AuditableEntity, IAggregateRoot
{
    public string? Name { get; private set; }
    public Guid AddressId { get; private set; }
    public virtual Address Address { get; private set; } = default!;

    private UserAddress()
    {
        // Required by ORM
    }

    public UserAddress(string name)
    {
        if (name?.Length == 0) throw new ArgumentNullException(nameof(name));

        Name = name;
    }
}