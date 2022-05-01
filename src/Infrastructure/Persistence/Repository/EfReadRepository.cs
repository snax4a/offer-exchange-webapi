using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using FSH.WebApi.Application.Common.Persistence;
using FSH.WebApi.Domain.Common.Contracts;
using FSH.WebApi.Infrastructure.Persistence.Context;
using FSH.WebApi.Infrastructure.Persistence.Specification.Evaluators;
using Mapster;

namespace FSH.WebApi.Infrastructure.Persistence.Repository;

// Inherited from Ardalis.Specification's RepositoryBase<T>
public class EfReadRepository<T> : RepositoryBase<T>, IReadRepository<T>
    where T : class, IEntity
{
    // We use custom evaluator to be able to use postgresql ILike expression in search queries
    // In postgresql Like expression is case sensitive so its not approperiate for searching
    public EfReadRepository(ApplicationDbContext dbContext)
        : base(dbContext, MySpecificationEvaluator.Instance)
    {
    }

    // We override the default behavior when mapping to a dto.
    // We're using Mapster's ProjectToType here to immediately map the result from the database.
    protected override IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification) =>
        ApplySpecification(specification, false)
            .ProjectToType<TResult>();
}