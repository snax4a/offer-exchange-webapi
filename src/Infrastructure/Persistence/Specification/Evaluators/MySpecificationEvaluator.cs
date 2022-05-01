using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;

namespace FSH.WebApi.Infrastructure.Persistence.Specification.Evaluators;

public sealed class MySpecificationEvaluator : SpecificationEvaluator
{
    public static MySpecificationEvaluator Instance { get; } = new MySpecificationEvaluator();

    private MySpecificationEvaluator()
        : base(new IEvaluator[]
        {
            WhereEvaluator.Instance,
            MySearchEvaluator.Instance,
            IncludeEvaluator.Default,
            OrderEvaluator.Instance,
            PaginationEvaluator.Instance,
            AsNoTrackingEvaluator.Instance,
            IgnoreQueryFiltersEvaluator.Instance,
            AsSplitQueryEvaluator.Instance,
            AsNoTrackingWithIdentityResolutionEvaluator.Instance
        })
    {
    }
}