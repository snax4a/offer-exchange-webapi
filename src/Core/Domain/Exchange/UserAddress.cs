namespace FSH.WebApi.Domain.Exchange;

public class UserAddress : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = default!;
    public Guid AddressId { get; private set; }
    public virtual Address Address { get; private set; } = default!;

    private UserAddress()
    {
        // Required by ORM
    }

    public UserAddress(string name, Address address)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

        Name = name;
        Address = address;
        AddressId = address.Id;
    }

    public UserAddress Update(string? name, Address? newAddress)
    {
        if (!string.IsNullOrEmpty(name) && Name?.Equals(name) is not true) Name = name;
        if (newAddress is not null && !Address.Equals(newAddress))
        {
            Address = newAddress;
            AddressId = newAddress.Id;
        }

        return this;
    }
}