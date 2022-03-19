using FSH.WebApi.Application.Exchange.Traders;

namespace FSH.WebApi.Host.Controllers.Exchange;

public class TradersController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Traders)]
    [OpenApiOperation("Search traders using available filters.", "")]
    public Task<PaginationResponse<TraderDetailsDto>> SearchAsync(SearchTradersRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View, FSHResource.Traders)]
    [OpenApiOperation("Get trader details.", "")]
    public Task<TraderDetailsDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetTraderRequest(id));
    }

    [HttpPost("get-by-groups")]
    [MustHavePermission(FSHAction.View, FSHResource.Traders)]
    [OpenApiOperation("Get unique traders which belong to any of the given group.", "")]
    public Task<List<TraderDto>> GetByGroupsAsync(GetTradersByGroupsRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Traders)]
    [OpenApiOperation("Create a new trader.", "")]
    public Task<Guid> CreateAsync(CreateTraderRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Traders)]
    [OpenApiOperation("Update trader.", "")]
    public async Task<ActionResult<Guid>> UpdateAsync(UpdateTraderRequest request, Guid id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.Traders)]
    [OpenApiOperation("Delete trader.", "")]
    public Task<Guid> DeleteAsync(Guid id)
    {
        return Mediator.Send(new DeleteTraderRequest(id));
    }
}