using FSH.WebApi.Application.Exchange.Addresses.DTOs;

namespace FSH.WebApi.Application.Exchange.Addresses.Specifications;

public class UserAddressByNameSpec : Specification<UserAddress, UserAddressDto>, ISingleResultSpecification
{
    public UserAddressByNameSpec(string name, Guid userId) =>
        Query
            .Where(ua => ua.Name == name && ua.CreatedBy == userId)
            .Include(ua => ua.Address);
}