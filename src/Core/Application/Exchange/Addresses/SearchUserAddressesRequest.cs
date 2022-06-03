using FSH.WebApi.Application.Exchange.Addresses.DTOs;
using FSH.WebApi.Application.Exchange.Addresses.Specifications;

namespace FSH.WebApi.Application.Exchange.Addresses;

public class SearchUserAddressesRequest : PaginationFilter, IRequest<PaginationResponse<UserAddressDto>>
{
}

public class SearchUserAddressesRequestHandler : IRequestHandler<SearchUserAddressesRequest, PaginationResponse<UserAddressDto>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IReadRepository<UserAddress> _repository;

    public SearchUserAddressesRequestHandler(ICurrentUser currentUser, IReadRepository<UserAddress> repository)
    {
        (_currentUser, _repository) = (currentUser, repository);
    }

    public async Task<PaginationResponse<UserAddressDto>> Handle(SearchUserAddressesRequest request, CancellationToken cancellationToken)
    {
        var spec = new SearchUserAddressesSpec(request, _currentUser.GetUserId());
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);
    }
}