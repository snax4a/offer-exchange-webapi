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
}