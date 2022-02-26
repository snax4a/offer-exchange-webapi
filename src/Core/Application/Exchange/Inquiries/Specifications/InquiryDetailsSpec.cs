namespace FSH.WebApi.Application.Exchange.Inquiries.Specifications;

public class InquiryDetailsSpec : Specification<Inquiry, InquiryDetailsDto>, ISingleResultSpecification
{
    public InquiryDetailsSpec(Guid id, Guid userId) =>
        Query
            .Where(i => i.Id == id && i.CreatedBy == userId)
            .Include(i => i.Products)
            .Include(i => i.InquiryRecipients);
}