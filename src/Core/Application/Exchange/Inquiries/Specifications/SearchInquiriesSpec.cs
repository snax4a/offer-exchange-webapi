namespace FSH.WebApi.Application.Exchange.Inquiries.Specifications;

public class SearchInquiriesSpec : EntitiesByPaginationFilterSpec<Inquiry, InquiryDto>
{
    public SearchInquiriesSpec(SearchInquiriesRequest request, Guid userId)
        : base(request) => Query
            .Where(i => i.CreatedBy == userId)
            .OrderByDescending(i => i.ReferenceNumber, !request.HasOrderBy());
}