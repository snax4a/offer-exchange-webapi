using FSH.WebApi.Application.Exchange.Offers;

namespace FSH.WebApi.Host.Controllers.Exchange;

public class OffersController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Offers)]
    [OpenApiOperation("Search offers using available filters.", "")]
    public Task<PaginationResponse<OfferDto>> SearchAsync(SearchOffersRequest request)
    {
        return Mediator.Send(request);
    }
}