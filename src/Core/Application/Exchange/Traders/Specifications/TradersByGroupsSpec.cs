namespace FSH.WebApi.Application.Exchange.Traders.Specifications;

public class TradersByGroupsSpec : Specification<Trader, TraderDto>
{
    public TradersByGroupsSpec(IList<Guid> groupIds, Guid userId) =>
        Query
            .Include(t => t.TraderGroups)
            .Where(t => t.CreatedBy == userId && t.TraderGroups.Any(tg => groupIds.Contains(tg.GroupId)));
}