namespace FSH.WebApi.Application.Exchange.Offers.Specifications;

public class SearchOffersSpec : EntitiesByPaginationFilterSpec<Offer, OfferWithInquiryDto>
{
    public SearchOffersSpec(SearchOffersRequest request, Guid userId)
        : base(request) => Query
            .Where(o => o.UserId == userId)
            .Where(o => o.TraderId == request.TraderId, request.TraderId != Guid.Empty && request.TraderId is not null)
            .Where(o => o.InquiryId == request.InquiryId, request.InquiryId != Guid.Empty && request.InquiryId is not null)
            .Where(o => o.HasReplacements == request.Replacements, request.Replacements is not null)
            .Where(o => o.HasFreebies == request.Freebies, request.Freebies is not null)
            .Include(o => o.Inquiry)
            .OrderBy(o => o.Id, !request.HasOrderBy());
}
