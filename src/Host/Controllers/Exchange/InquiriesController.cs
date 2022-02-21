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
}