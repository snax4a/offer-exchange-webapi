using FSH.WebApi.Application.Comparison;
using FSH.WebApi.Application.Comparison.DTOs;

namespace FSH.WebApi.Host.Controllers.Comparison;

public class ComparisonController : VersionedApiController
{
    [HttpGet("product/{inquiryProductId:guid}/offers")]
    [MustHavePermission(ResourceAction.View, Resource.Inquiries)]
    [OpenApiOperation("Get inquiry product offers.", "")]
    public Task<IEnumerable<InquiryProductOfferDto>> GetProductOffersAsync(Guid inquiryProductId)
    {
        return Mediator.Send(new GetProductOffersRequest(inquiryProductId));
    }

    [HttpPost("get-best-offers")]
    [MustHavePermission(ResourceAction.View, Resource.Inquiries)]
    [OpenApiOperation("Get list of the best offers for all inquiry products.", "")]
    public Task<IEnumerable<InquiryProductOfferDto>> PrepareOrdersForInquiryAsync(GetTheBestOffersForInquiryRequest request)
    {
        return Mediator.Send(request);
    }
}