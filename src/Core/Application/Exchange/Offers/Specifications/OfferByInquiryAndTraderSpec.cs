namespace FSH.WebApi.Application.Exchange.Offers.Specifications;

public class OfferByInquiryAndTraderSpec : Specification<Offer, OfferDto>, ISingleResultSpecification
{
    public OfferByInquiryAndTraderSpec(Guid inquiryId, Guid traderId) =>
        Query.Where(o => o.InquiryId == inquiryId && o.TraderId == traderId);
}
