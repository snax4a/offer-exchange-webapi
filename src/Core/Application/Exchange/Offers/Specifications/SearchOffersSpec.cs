namespace FSH.WebApi.Application.Exchange.Offers.Specifications;

public class SearchOffersSpec : EntitiesByPaginationFilterSpec<Offer, OfferDto>
{
    public SearchOffersSpec(SearchOffersRequest request, Guid userId)
        : base(request) => Query
            .Where(o => o.UserId == userId)
            .OrderBy(o => o.Id, !request.HasOrderBy());
}
