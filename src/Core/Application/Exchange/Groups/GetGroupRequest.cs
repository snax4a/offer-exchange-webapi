namespace FSH.WebApi.Application.Exchange.Groups;

public class GetGroupRequest : IRequest<GroupDto>
{
    public Guid Id { get; set; }

    public GetGroupRequest(Guid id) => Id = id;
}

public class GroupByIdSpec : Specification<Group, GroupDto>, ISingleResultSpecification
{
    public GroupByIdSpec(Guid id, Guid userId) =>
        Query.Where(g => g.Id == id && g.CreatedBy == userId);
}

public class GetGroupRequestHandler : IRequestHandler<GetGroupRequest, GroupDto>
{
    private readonly ICurrentUser _currentUser;
    private readonly IRepository<Group> _repository;
    private readonly IStringLocalizer<GetGroupRequestHandler> _localizer;

    public GetGroupRequestHandler(ICurrentUser currentUser, IRepository<Group> repository, IStringLocalizer<GetGroupRequestHandler> localizer) => (_currentUser, _repository, _localizer) = (currentUser, repository, localizer);

    public async Task<GroupDto> Handle(GetGroupRequest request, CancellationToken cancellationToken)
    {
        ISpecification<Group, GroupDto> spec = new GroupByIdSpec(request.Id, _currentUser.GetUserId());
        var group = await _repository.GetBySpecAsync(spec, cancellationToken);

        if (group is not null) return group;

        throw new NotFoundException(string.Format(_localizer["group.notfound"], request.Id));
    }
}