using FSH.WebApi.Application.Exchange.Addresses.DTOs;

namespace FSH.WebApi.Application.Exchange.Addresses.Specifications;

public class UserAddressByIdSpec : Specification<UserAddress, UserAddressDto>, ISingleResultSpecification
{
    public UserAddressByIdSpec(Guid id, Guid userId) =>
        Query
            .Where(ua => ua.Id == id && ua.CreatedBy == userId)
            .Include(ua => ua.Address);
}