namespace FSH.WebApi.Application.Exchange.Offers.Specifications;

public class OfferProductForOrderSpec : Specification<OfferProduct>, ISingleResultSpecification
{
    public OfferProductForOrderSpec(Guid offerProductId, Guid userId) =>
        Query
            .Where(op => op.Id == offerProductId && op.Offer.UserId == userId)
            .Include(op => op.Offer)
                .ThenInclude(o => o.Trader);
}
