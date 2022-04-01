using FSH.WebApi.Application.Exchange.Groups.DTOs;

namespace FSH.WebApi.Application.Exchange.Groups.Specifications;

public class GroupDetailsSpec : Specification<Group, GroupDetailsDto>, ISingleResultSpecification
{
    public GroupDetailsSpec(Guid id, Guid userId) =>
        Query
            .Where(t => t.Id == id && t.CreatedBy == userId)
            .Include(t => t.TraderGroups)
                .ThenInclude(tg => tg.Trader);
}