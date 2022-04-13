using FSH.WebApi.Application.Exchange.Inquiries.DTOs;
using FSH.WebApi.Application.Exchange.Inquiries.Specifications;

namespace FSH.WebApi.Application.Exchange.Inquiries;

public class GetInquiryProductsRequest : IRequest<IEnumerable<InquiryProductDetailsDto>>
{
    public Guid InquiryId { get; set; }

    public GetInquiryProductsRequest(Guid inquiryId) => InquiryId = inquiryId;
}

public class GetInquiryProductsRequestHandler : IRequestHandler<GetInquiryProductsRequest, IEnumerable<InquiryProductDetailsDto>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IReadRepository<InquiryProduct> _repository;

    public GetInquiryProductsRequestHandler(ICurrentUser currentUser, IReadRepository<InquiryProduct> repository)
    {
        (_currentUser, _repository) = (currentUser, repository);
    }

    public async Task<IEnumerable<InquiryProductDetailsDto>> Handle(GetInquiryProductsRequest request, CancellationToken cancellationToken)
    {
        var spec = new InquiryProductsByInquiryIdSpec(request.InquiryId, _currentUser.GetUserId());
        return await _repository.ListAsync(spec, cancellationToken);
    }
}