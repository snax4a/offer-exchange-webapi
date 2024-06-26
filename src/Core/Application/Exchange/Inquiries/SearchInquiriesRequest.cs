using FSH.WebApi.Application.Exchange.Inquiries.DTOs;
using FSH.WebApi.Application.Exchange.Inquiries.Specifications;

namespace FSH.WebApi.Application.Exchange.Inquiries;

public class SearchInquiriesRequest : PaginationFilter, IRequest<PaginationResponse<InquiryWithCountsDto>>
{
    public Guid? TraderId { get; set; }
}

public class SearchInquiriesRequestHandler : IRequestHandler<SearchInquiriesRequest, PaginationResponse<InquiryWithCountsDto>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IReadRepository<Inquiry> _repository;

    public SearchInquiriesRequestHandler(ICurrentUser currentUser, IReadRepository<Inquiry> repository)
    {
        (_currentUser, _repository) = (currentUser, repository);
    }

    public async Task<PaginationResponse<InquiryWithCountsDto>> Handle(SearchInquiriesRequest request, CancellationToken cancellationToken)
    {
        var spec = new SearchInquiriesSpec(request, _currentUser.GetUserId());
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);
    }
}