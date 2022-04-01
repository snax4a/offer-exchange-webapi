using FSH.WebApi.Application.Exchange.Inquiries.DTOs;

namespace FSH.WebApi.Application.Exchange.Inquiries.Specifications;

public class SearchInquiriesSpec : EntitiesByPaginationFilterSpec<Inquiry, InquiryWithCountsDto>
{
    public SearchInquiriesSpec(SearchInquiriesRequest request, Guid userId)
        : base(request) => Query
            .Where(i => i.CreatedBy == userId)
            .Where(i => i.InquiryRecipients.Any(ir => ir.TraderId == request.TraderId), request.TraderId != Guid.Empty && request.TraderId is not null)
            .OrderByDescending(i => i.ReferenceNumber, !request.HasOrderBy());
}