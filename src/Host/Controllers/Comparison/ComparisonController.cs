using FSH.WebApi.Application.Comparison.DTOs;
using FSH.WebApi.Application.Exchange.Comparison;

namespace FSH.WebApi.Host.Controllers.Comparison;

public class ComparisonController : VersionedApiController
{
    [HttpGet("product/{inquiryProductId:guid}/offers")]
    [MustHavePermission(FSHAction.View, FSHResource.Inquiries)]
    [OpenApiOperation("Get inquiry product offers.", "")]
    public Task<ProductOfferListDto> GetProductOffersAsync(Guid inquiryProductId)
    {
        return Mediator.Send(new GetProductOffersRequest(inquiryProductId));
    }
}