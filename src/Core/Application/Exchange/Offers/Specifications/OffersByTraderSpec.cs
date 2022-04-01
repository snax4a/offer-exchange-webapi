using FSH.WebApi.Application.Exchange.Offers.DTOs;

namespace FSH.WebApi.Application.Exchange.Offers.Specifications;

public class OffersByTraderSpec : Specification<Offer>
{
    public OffersByTraderSpec(Guid traderId) =>
        Query.Where(o => o.TraderId == traderId);
}