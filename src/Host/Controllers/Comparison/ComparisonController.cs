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
}