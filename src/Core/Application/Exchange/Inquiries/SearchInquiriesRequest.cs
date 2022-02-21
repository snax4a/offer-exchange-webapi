namespace FSH.WebApi.Application.Exchange.Inquiries;

public class SearchInquiriesRequest : PaginationFilter, IRequest<PaginationResponse<InquiryDto>>
{
}

public class SearchInquiriesSpec : EntitiesByPaginationFilterSpec<Inquiry, InquiryDto>
{
    public SearchInquiriesSpec(SearchInquiriesRequest request, Guid userId)
        : base(request) => Query
            .Where(i => i.CreatedBy == userId)
            .OrderByDescending(i => i.ReferenceNumber, !request.HasOrderBy());
}

public class SearchInquiriesRequestHandler : IRequestHandler<SearchInquiriesRequest, PaginationResponse<InquiryDto>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IReadRepository<Inquiry> _repository;

    public SearchInquiriesRequestHandler(ICurrentUser currentUser, IReadRepository<Inquiry> repository) =>
        (_currentUser, _repository) = (currentUser, repository);

    public async Task<PaginationResponse<InquiryDto>> Handle(SearchInquiriesRequest request, CancellationToken cancellationToken)
    {
        var spec = new SearchInquiriesSpec(request, _currentUser.GetUserId());

        var list = await _repository.ListAsync(spec, cancellationToken);
        int count = await _repository.CountAsync(spec, cancellationToken);

        return new PaginationResponse<InquiryDto>(list, count, request.PageNumber, request.PageSize);
    }
}