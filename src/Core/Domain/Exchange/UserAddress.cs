using FSH.WebApi.Core.Shared.Extensions;

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
        string strippedName = name.StripHtml();

        if (string.IsNullOrEmpty(strippedName)) throw new ArgumentNullException(nameof(name));

        Name = strippedName;
        Address = address;
        AddressId = address.Id;
    }

    public UserAddress Update(string? name, Address? newAddress)
    {
        string? strippedName = name?.StripHtml();

        if (!string.IsNullOrEmpty(strippedName) && Name?.Equals(strippedName) is not true) Name = strippedName;
        if (newAddress is not null && !Address.Equals(newAddress))
        {
            Address = newAddress;
            AddressId = newAddress.Id;
        }

        return this;
    }
}