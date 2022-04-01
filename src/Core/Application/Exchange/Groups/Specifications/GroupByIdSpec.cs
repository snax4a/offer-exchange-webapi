using FSH.WebApi.Application.Exchange.Groups.DTOs;

namespace FSH.WebApi.Application.Exchange.Groups.Specifications;

public class GroupByIdSpec : Specification<Group, GroupDto>, ISingleResultSpecification
{
    public GroupByIdSpec(Guid id, Guid userId) =>
        Query.Where(g => g.Id == id && g.CreatedBy == userId);
}