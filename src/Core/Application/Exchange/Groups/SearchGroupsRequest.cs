namespace FSH.WebApi.Application.Exchange.Groups;

public class SearchGroupsRequest : PaginationFilter, IRequest<PaginationResponse<GroupDto>>
{
}

public class SearchGroupsSpec : EntitiesByPaginationFilterSpec<Group, GroupDto>
{
    public SearchGroupsSpec(SearchGroupsRequest request, Guid userId)
        : base(request) => Query
            .Where(g => g.CreatedBy == userId)
            .OrderBy(g => g.Name, !request.HasOrderBy());
}

public class SearchGroupsRequestHandler : IRequestHandler<SearchGroupsRequest, PaginationResponse<GroupDto>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IReadRepository<Group> _repository;

    public SearchGroupsRequestHandler(ICurrentUser currentUser, IReadRepository<Group> repository) =>
        (_currentUser, _repository) = (currentUser, repository);

    public async Task<PaginationResponse<GroupDto>> Handle(SearchGroupsRequest request, CancellationToken cancellationToken)
    {
        var spec = new SearchGroupsSpec(request, _currentUser.GetUserId());

        var list = await _repository.ListAsync(spec, cancellationToken);
        int count = await _repository.CountAsync(spec, cancellationToken);

        return new PaginationResponse<GroupDto>(list, count, request.PageNumber, request.PageSize);
    }
}