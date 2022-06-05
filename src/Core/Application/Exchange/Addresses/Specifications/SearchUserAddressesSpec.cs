using FSH.WebApi.Application.Exchange.Addresses.DTOs;

namespace FSH.WebApi.Application.Exchange.Addresses.Specifications;

public class SearchUserAddressesSpec : EntitiesByPaginationFilterSpec<UserAddress, UserAddressDto>
{
    public SearchUserAddressesSpec(SearchUserAddressesRequest request, Guid userId)
        : base(request) => Query
            .Where(ua => ua.CreatedBy == userId)
            .Include(ua => ua.Address)
                .ThenInclude(ua => ua.Country)
            .OrderByDescending(ua => ua.Name, !request.HasOrderBy());
}