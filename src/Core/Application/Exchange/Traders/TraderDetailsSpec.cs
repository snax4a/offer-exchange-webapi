namespace FSH.WebApi.Application.Exchange.Traders;

public class TraderDetailsSpec : Specification<Trader, TraderDetailsDto>, ISingleResultSpecification
{
    public TraderDetailsSpec(Guid id, Guid userId) =>
        Query
            .Where(t => t.Id == id && t.CreatedBy == userId)
            .Include(t => t.TraderGroups);
}