namespace FSH.WebApi.Application.Exchange.Offers;

public class OffersByTraderSpec : Specification<Offer>
{
    public OffersByTraderSpec(Guid traderId) =>
        Query.Where(o => o.TraderId == traderId);
}