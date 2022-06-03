using FSH.WebApi.Application.Exchange.Addresses.DTOs;
using FSH.WebApi.Application.Exchange.Addresses.Specifications;

namespace FSH.WebApi.Application.Exchange.Addresses;

public class GetUserAddressDetailsRequest : IRequest<UserAddressDto>
{
    public Guid Id { get; set; }

    public GetUserAddressDetailsRequest(Guid id) => Id = id;
}

public class GetUserAddressDetailsRequestHandler : IRequestHandler<GetUserAddressDetailsRequest, UserAddressDto>
{
    private readonly ICurrentUser _currentUser;
    private readonly IRepository<UserAddress> _repository;
    private readonly IStringLocalizer<GetUserAddressDetailsRequestHandler> _localizer;

    public GetUserAddressDetailsRequestHandler(
        ICurrentUser currentUser,
        IRepository<UserAddress> repository,
        IStringLocalizer<GetUserAddressDetailsRequestHandler> localizer)
    {
        (_currentUser, _repository, _localizer) = (currentUser, repository, localizer);
    }

    public async Task<UserAddressDto> Handle(GetUserAddressDetailsRequest request, CancellationToken cancellationToken)
    {
        ISpecification<UserAddress, UserAddressDto> spec = new UserAddressByIdSpec(request.Id, _currentUser.GetUserId());
        var userAddress = await _repository.GetBySpecAsync(spec, cancellationToken);

        if (userAddress is not null) return userAddress;

        throw new NotFoundException(_localizer["address.notfound"]);
    }
}