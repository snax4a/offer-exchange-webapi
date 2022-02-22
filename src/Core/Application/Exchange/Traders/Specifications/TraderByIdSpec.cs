namespace FSH.WebApi.Application.Exchange.Traders.Specifications;

public class TraderByIdSpec : Specification<Trader>, ISingleResultSpecification
{
    public TraderByIdSpec(Guid id) => Query.Where(t => t.Id == id);
}