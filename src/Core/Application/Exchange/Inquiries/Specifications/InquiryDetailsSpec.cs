using FSH.WebApi.Application.Exchange.Inquiries.DTOs;

namespace FSH.WebApi.Application.Exchange.Inquiries.Specifications;

public class InquiryDetailsSpec : Specification<Inquiry, InquiryDetailsDto>, ISingleResultSpecification
{
    public InquiryDetailsSpec(Guid inquiryId, Guid userId) =>
        Query
            .Where(i => i.Id == inquiryId && i.CreatedBy == userId)
            .Include(i => i.Products)
            .Include(i => i.InquiryRecipients);

    public InquiryDetailsSpec(Guid inquiryId) =>
        Query
            .Where(i => i.Id == inquiryId)
            .Include(i => i.Products)
            .Include(i => i.InquiryRecipients);
}