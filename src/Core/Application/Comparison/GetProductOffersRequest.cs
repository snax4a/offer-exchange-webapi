using FSH.WebApi.Application.Comparison.DTOs;

namespace FSH.WebApi.Application.Comparison;

public class GetProductOffersRequest : IRequest<IEnumerable<InquiryProductOfferDto>>
{
    public Guid InquiryProductId { get; set; }

    public GetProductOffersRequest(Guid inquiryProductId)
        => InquiryProductId = inquiryProductId;
}

public class GetProductOffersRequestHandler : IRequestHandler<GetProductOffersRequest, IEnumerable<InquiryProductOfferDto>>
{
    private readonly IComparisonRepository _repository;

    public GetProductOffersRequestHandler(IComparisonRepository repository) => _repository = repository;

    public async Task<IEnumerable<InquiryProductOfferDto>> Handle(GetProductOffersRequest request, CancellationToken ct)
    {
        return await _repository.GetOffersForInquiryProductAsync(request.InquiryProductId, ct);
    }
}