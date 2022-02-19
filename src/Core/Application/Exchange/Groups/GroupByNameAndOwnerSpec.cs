namespace FSH.WebApi.Application.Exchange.Groups;

public class GroupByNameAndOwnerSpec : Specification<Group>, ISingleResultSpecification
{
    public GroupByNameAndOwnerSpec(string name, Guid ownerId) =>
        Query.Where(g => g.Name == name && g.CreatedBy == ownerId);
}