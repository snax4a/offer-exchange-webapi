using FSH.WebApi.Application.Exchange.Inquiries;

namespace FSH.WebApi.Host.Controllers.Exchange;

public class InquiriesController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Inquiries)]
    [OpenApiOperation("Search inquiries using available filters.", "")]
    public Task<PaginationResponse<InquiryDto>> SearchAsync(SearchInquiriesRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View, FSHResource.Inquiries)]
    [OpenApiOperation("Get inquiry details.", "")]
    public Task<InquiryDetailsDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetInquiryRequest(id));
    }

    [HttpGet("get-by-offer-token/{offerToken}")]
    [AllowAnonymous]
    [TenantIdHeader]
    [OpenApiOperation("Get inquiry details for offer form.", "")]
    public Task<InquiryForOfferDto> GetByOfferTokenAsync(string offerToken)
    {
        return Mediator.Send(new GetInquiryForOfferRequest(offerToken));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Inquiries)]
    [OpenApiOperation("Create a new inquiry.", "")]
    public Task<Guid> CreateAsync(CreateInquiryRequest request)
    {
        return Mediator.Send(request);
    }
}