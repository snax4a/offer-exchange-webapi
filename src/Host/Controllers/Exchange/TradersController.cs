using FSH.WebApi.Application.Exchange.Traders;

namespace FSH.WebApi.Host.Controllers.Exchange;

public class TradersController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Traders)]
    [OpenApiOperation("Search traders using available filters.", "")]
    public Task<PaginationResponse<TraderDto>> SearchAsync(SearchTradersRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View, FSHResource.Traders)]
    [OpenApiOperation("Get trader details.", "")]
    public Task<TraderDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetTraderRequest(id));
    }
}