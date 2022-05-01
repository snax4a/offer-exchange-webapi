using Ardalis.Specification;
using FSH.WebApi.Infrastructure.Persistence.Specification.Extensions;

namespace FSH.WebApi.Infrastructure.Persistence.Specification.Evaluators;
public sealed class MySearchEvaluator : IEvaluator
{
    private MySearchEvaluator()
    {
    }

    public static MySearchEvaluator Instance { get; } = new MySearchEvaluator();

    public bool IsCriteriaEvaluator { get; } = true;

    public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification)
        where T : class
    {
        foreach (var searchCriteria in specification.SearchCriterias.GroupBy(x => x.SearchGroup))
        {
            query = query.Search(searchCriteria);
        }

        return query;
    }
}