using FSH.WebApi.Application.Exchange.Offers.DTOs;

namespace FSH.WebApi.Application.Exchange.Offers.Specifications;

public class OfferByInquiryAndTraderSpec : Specification<Offer, OfferDetailsDto>, ISingleResultSpecification
{
    public OfferByInquiryAndTraderSpec(Guid inquiryId, Guid traderId) =>
        Query
            .Where(o => o.InquiryId == inquiryId && o.TraderId == traderId)
            .Include(o => o.OfferProducts)
            .Include(o => o.ShippingAddress!)
                .ThenInclude(a => a.Country);
}
