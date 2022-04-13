using FSH.WebApi.Application.Exchange.Inquiries.DTOs;

namespace FSH.WebApi.Application.Exchange.Inquiries.Specifications;

public class InquiryProductsByInquiryIdSpec : Specification<InquiryProduct, InquiryProductDetailsDto>
{
    public InquiryProductsByInquiryIdSpec(Guid inquiryId, Guid userId) =>
        Query.Where(i => i.InquiryId == inquiryId && i.CreatedBy == userId);
}