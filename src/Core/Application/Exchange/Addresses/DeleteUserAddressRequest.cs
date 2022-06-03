using FSH.WebApi.Application.Exchange.Addresses.Specifications;

namespace FSH.WebApi.Application.Exchange.Addresses;

public class DeleteUserAddressRequest : IRequest<Guid>
{
    public Guid Id { get; set; }

    public DeleteUserAddressRequest(Guid id) => Id = id;
}

public class DeleteGroupReAddressHandler : IRequestHandler<DeleteUserAddressRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<UserAddress> _repository;
    private readonly ICurrentUser _currentUser;
    private readonly IStringLocalizer<DeleteGroupReAddressHandler> _localizer;

    public DeleteGroupReAddressHandler(
        IRepositoryWithEvents<UserAddress> repository,
        ICurrentUser currentUser,
        IStringLocalizer<DeleteGroupReAddressHandler> localizer)
    {
        (_repository, _currentUser, _localizer) = (repository, currentUser, localizer);
    }

    public async Task<Guid> Handle(DeleteUserAddressRequest request, CancellationToken cancellationToken)
    {
        var spec = new UserAddressByIdSpec(request.Id, _currentUser.GetUserId());
        var userAddress = await _repository.GetBySpecAsync(spec, cancellationToken);

        _ = userAddress ?? throw new NotFoundException(_localizer["address.notfound"]);

        await _repository.DeleteAsync(userAddress, cancellationToken);

        return request.Id;
    }
}