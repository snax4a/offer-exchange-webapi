namespace FSH.WebApi.Application.Exchange.Traders;

public class TraderByEmailSpec : Specification<Trader>, ISingleResultSpecification
{
    public TraderByEmailSpec(string email, Guid ownerId) =>
        Query.Where(g => g.Email == email && g.CreatedBy == ownerId);
}