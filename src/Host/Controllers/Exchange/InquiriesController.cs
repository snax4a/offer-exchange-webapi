using FSH.WebApi.Application.Exchange.Inquiries;
using FSH.WebApi.Application.Exchange.Inquiries.DTOs;

namespace FSH.WebApi.Host.Controllers.Exchange;

public class InquiriesController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(ResourceAction.Search, Resource.Inquiries)]
    [OpenApiOperation("Search inquiries using available filters.", "")]
    public Task<PaginationResponse<InquiryWithCountsDto>> SearchAsync(SearchInquiriesRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(ResourceAction.View, Resource.Inquiries)]
    [OpenApiOperation("Get inquiry details.", "")]
    public Task<InquiryDetailsDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetInquiryRequest(id));
    }

    [HttpGet("{id:guid}/products")]
    [MustHavePermission(ResourceAction.View, Resource.Inquiries)]
    [OpenApiOperation("Get inquiry products.", "")]
    public Task<IEnumerable<InquiryProductDetailsDto>> GetProductsAsync(Guid id)
    {
        return Mediator.Send(new GetInquiryProductsRequest(id));
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
    [MustHavePermission(ResourceAction.Create, Resource.Inquiries)]
    [OpenApiOperation("Create a new inquiry.", "")]
    public Task<Guid> CreateAsync(CreateInquiryRequest request)
    {
        return Mediator.Send(request);
    }
}