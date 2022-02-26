namespace FSH.WebApi.Application.Exchange.Inquiries.Specifications;

public class InquiryByIdAndTraderSpec : Specification<Inquiry, InquiryDetailsDto>, ISingleResultSpecification
{
    public InquiryByIdAndTraderSpec(Guid inquiryId, Guid traderId) =>
        Query
            .Include(i => i.InquiryRecipients)
            .Where(i => i.Id == inquiryId && i.InquiryRecipients.Any(ir => ir.TraderId == traderId));
}