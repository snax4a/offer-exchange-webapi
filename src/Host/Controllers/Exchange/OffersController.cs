using FSH.WebApi.Application.Exchange.Offers;

namespace FSH.WebApi.Host.Controllers.Exchange;

public class OffersController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Offers)]
    [OpenApiOperation("Search offers using available filters.", "")]
    public Task<PaginationResponse<OfferWithInquiryDto>> SearchAsync(SearchOffersRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View, FSHResource.Offers)]
    [OpenApiOperation("Get offer details.", "")]
    public Task<OfferDetailsDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetOfferRequest(id));
    }

    [HttpPost]
    [AllowAnonymous]
    [OpenApiOperation("Create a new offer.", "")]
    public Task<Guid> CreateAsync(CreateOfferRequest request)
    {
        return Mediator.Send(request);
    }
}