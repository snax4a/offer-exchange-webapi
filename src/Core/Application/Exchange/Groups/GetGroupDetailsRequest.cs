using FSH.WebApi.Application.Exchange.Groups.Specifications;

namespace FSH.WebApi.Application.Exchange.Groups;

public class GetGroupDetailsRequest : IRequest<GroupDetailsDto>
{
    public Guid Id { get; set; }

    public GetGroupDetailsRequest(Guid id) => Id = id;
}

public class GetGroupDetailsRequestHandler : IRequestHandler<GetGroupDetailsRequest, GroupDetailsDto>
{
    private readonly ICurrentUser _currentUser;
    private readonly IRepository<Group> _repository;
    private readonly IStringLocalizer<GetGroupDetailsRequestHandler> _localizer;

    public GetGroupDetailsRequestHandler(
        ICurrentUser currentUser,
        IRepository<Group> repository,
        IStringLocalizer<GetGroupDetailsRequestHandler> localizer)
    {
        (_currentUser, _repository, _localizer) = (currentUser, repository, localizer);
    }

    public async Task<GroupDetailsDto> Handle(GetGroupDetailsRequest request, CancellationToken cancellationToken)
    {
        ISpecification<Group, GroupDetailsDto> spec = new GroupDetailsSpec(request.Id, _currentUser.GetUserId());
        var group = await _repository.GetBySpecAsync(spec, cancellationToken);

        if (group is not null) return group;

        throw new NotFoundException(string.Format(_localizer["group.notfound"], request.Id));
    }
}