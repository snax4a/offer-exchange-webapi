namespace FSH.WebApi.Application.Exchange.Inquiries.Specifications;

public class UserInquiriesSpec : Specification<Inquiry>, ISingleResultSpecification
{
    public UserInquiriesSpec(Guid userId) => Query.Where(i => i.CreatedBy == userId);
}