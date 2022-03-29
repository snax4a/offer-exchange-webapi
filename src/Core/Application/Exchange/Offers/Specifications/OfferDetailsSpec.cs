namespace FSH.WebApi.Application.Exchange.Offers.Specifications;

public class OfferDetailsSpec : Specification<Offer, OfferDetailsDto>, ISingleResultSpecification
{
    public OfferDetailsSpec(Guid id, Guid userId) =>
        Query
            .Where(o => o.Id == id && o.UserId == userId)
            .Include(o => o.OfferProducts)
            .Include(o => o.Inquiry);
}
