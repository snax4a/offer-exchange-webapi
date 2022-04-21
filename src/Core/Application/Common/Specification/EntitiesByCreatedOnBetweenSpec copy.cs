namespace FSH.WebApi.Application.Common.Specification;

public class EntitiesByCreatedOnBetweenSpec<T> : Specification<T>
    where T : ICreatedOnInformation
{
    public EntitiesByCreatedOnBetweenSpec(DateTime from, DateTime until) =>
        Query.Where(e => e.CreatedOn >= from && e.CreatedOn <= until);
}