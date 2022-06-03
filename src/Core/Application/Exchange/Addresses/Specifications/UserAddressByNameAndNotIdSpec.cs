using FSH.WebApi.Application.Exchange.Addresses.DTOs;

namespace FSH.WebApi.Application.Exchange.Addresses.Specifications;

public class UserAddressByNameAndNotIdSpec : Specification<UserAddress, UserAddressDto>, ISingleResultSpecification
{
    public UserAddressByNameAndNotIdSpec(string name, Guid id, Guid userId) =>
        Query
            .Where(ua => ua.Id != id)
            .Where(ua => ua.Name == name && ua.CreatedBy == userId)
            .Include(ua => ua.Address);
}