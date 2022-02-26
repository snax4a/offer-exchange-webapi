namespace FSH.WebApi.Application.Exchange.Groups.Specifications;

public class SearchGroupsSpec : EntitiesByPaginationFilterSpec<Group, GroupDto>
{
    public SearchGroupsSpec(SearchGroupsRequest request, Guid userId)
        : base(request) => Query
            .Where(g => g.CreatedBy == userId)
            .OrderBy(g => g.Name, !request.HasOrderBy());
}