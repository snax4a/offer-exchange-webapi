namespace FSH.WebApi.Application.Exchange.Traders.Specifications;
public class SearchTradersSpec : EntitiesByPaginationFilterSpec<Trader, TraderDetailsDto>
{
    public SearchTradersSpec(SearchTradersRequest request, Guid userId)
        : base(request) => Query
            .Where(t => t.CreatedBy == userId)
            .Include(t => t.TraderGroups)
                .ThenInclude(tg => tg.Group)
            .OrderBy(t => t.FirstName, !request.HasOrderBy())
            .ThenBy(t => t.LastName, !request.HasOrderBy());
}