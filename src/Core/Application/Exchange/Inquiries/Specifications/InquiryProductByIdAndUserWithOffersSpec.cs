namespace FSH.WebApi.Application.Exchange.Inquiries.Specifications;

public class InquiryProductByIdAndUserWithOffersSpec : Specification<InquiryProduct>, ISingleResultSpecification
{
    public InquiryProductByIdAndUserWithOffersSpec(Guid id, Guid userId) =>
        Query
            .Where(ip => ip.Id == id && ip.CreatedBy == userId)
            .Include(ip => ip.OfferProducts);
}