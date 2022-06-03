namespace FSH.WebApi.Application.Exchange.Addresses.DTOs;

public class UserAddressDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; private set; } = default!;
    public AddressDto Address { get; set; } = default!;
}