namespace FSH.WebApi.Application.Exchange.Traders;

public class TradersByGroupSpec : Specification<Trader>
{
    public TradersByGroupSpec(Guid groupId) =>
        Query
            .Include(t => t.TraderGroups)
            .Where(t => t.TraderGroups.Any(tg => tg.GroupId == groupId));

    public TradersByGroupSpec(Guid groupId, Guid userId) =>
        Query
            .Include(t => t.TraderGroups)
            .Where(t => t.CreatedBy == userId && t.TraderGroups.Any(tg => tg.GroupId == groupId));
}