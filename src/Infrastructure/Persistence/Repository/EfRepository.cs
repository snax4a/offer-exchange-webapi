﻿using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using FSH.WebApi.Application.Common.Persistence;
using FSH.WebApi.Domain.Common.Contracts;
using FSH.WebApi.Infrastructure.Persistence.Context;
using Mapster;

namespace FSH.WebApi.Infrastructure.Persistence.Repository;

// Inherited from Ardalis.Specification's RepositoryBase<T>
public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
{
    public EfRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }

    // We override the default behavior when mapping to a dto.
    // We're using Mapster's ProjectToType here to immediately map the result from the database.
    protected override IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification) =>
        specification.Selector is not null
            ? base.ApplySpecification(specification)
            : ApplySpecification(specification, false)
                .ProjectToType<TResult>();
}