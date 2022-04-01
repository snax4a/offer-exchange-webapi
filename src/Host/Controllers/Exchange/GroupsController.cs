using FSH.WebApi.Application.Exchange.Groups;
using FSH.WebApi.Application.Exchange.Groups.DTOs;

namespace FSH.WebApi.Host.Controllers.Exchange;

public class GroupsController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Groups)]
    [OpenApiOperation("Search groups using available filters.", "")]
    public Task<PaginationResponse<GroupDto>> SearchAsync(SearchGroupsRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View, FSHResource.Groups)]
    [OpenApiOperation("Get group details.", "")]
    public Task<GroupDetailsDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetGroupDetailsRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Groups)]
    [OpenApiOperation("Create a new group.", "")]
    public Task<Guid> CreateAsync(CreateGroupRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Groups)]
    [OpenApiOperation("Update group.", "")]
    public async Task<ActionResult<Guid>> UpdateAsync(UpdateGroupRequest request, Guid id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.Groups)]
    [OpenApiOperation("Delete group.", "")]
    public Task<Guid> DeleteAsync(Guid id)
    {
        return Mediator.Send(new DeleteGroupRequest(id));
    }
}