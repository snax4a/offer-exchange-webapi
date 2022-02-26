using FSH.WebApi.Application.Exchange.Groups.Specifications;

namespace FSH.WebApi.Application.Exchange.Groups;

public class SearchGroupsRequest : PaginationFilter, IRequest<PaginationResponse<GroupDto>>
{
}

public class SearchGroupsRequestHandler : IRequestHandler<SearchGroupsRequest, PaginationResponse<GroupDto>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IReadRepository<Group> _repository;

    public SearchGroupsRequestHandler(ICurrentUser currentUser, IReadRepository<Group> repository)
    {
        (_currentUser, _repository) = (currentUser, repository);
    }

    public async Task<PaginationResponse<GroupDto>> Handle(SearchGroupsRequest request, CancellationToken cancellationToken)
    {
        var spec = new SearchGroupsSpec(request, _currentUser.GetUserId());
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);
    }
}