using FSH.WebApi.Application.Exchange.Groups;
using FSH.WebApi.Application.Exchange.Groups.DTOs;

namespace FSH.WebApi.Host.Controllers.Exchange;

public class GroupsController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(ResourceAction.Search, Resource.Groups)]
    [OpenApiOperation("Search groups using available filters.", "")]
    public Task<PaginationResponse<GroupDto>> SearchAsync(SearchGroupsRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(ResourceAction.View, Resource.Groups)]
    [OpenApiOperation("Get group details.", "")]
    public Task<GroupDetailsDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetGroupDetailsRequest(id));
    }

    [HttpPost]
    [MustHavePermission(ResourceAction.Create, Resource.Groups)]
    [OpenApiOperation("Create a new group.", "")]
    public Task<Guid> CreateAsync(CreateGroupRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(ResourceAction.Update, Resource.Groups)]
    [OpenApiOperation("Update group.", "")]
    public async Task<ActionResult<Guid>> UpdateAsync(UpdateGroupRequest request, Guid id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(ResourceAction.Delete, Resource.Groups)]
    [OpenApiOperation("Delete group.", "")]
    public Task<Guid> DeleteAsync(Guid id)
    {
        return Mediator.Send(new DeleteGroupRequest(id));
    }
}