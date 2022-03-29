namespace FSH.WebApi.Application.Exchange.Traders.Specifications;
public class SearchTradersSpec : EntitiesByPaginationFilterSpec<Trader, TraderDetailsDto>
{
    public SearchTradersSpec(SearchTradersRequest request, Guid userId)
        : base(request) => Query
            .Where(t => t.CreatedBy == userId)
            .Where(t => t.TraderGroups.Any(tg => tg.GroupId == request.GroupId), request.GroupId != Guid.Empty && request.GroupId is not null)
            .Include(t => t.TraderGroups)
                .ThenInclude(tg => tg.Group)
            .OrderBy(t => t.FirstName, !request.HasOrderBy())
            .ThenBy(t => t.LastName, !request.HasOrderBy());
}