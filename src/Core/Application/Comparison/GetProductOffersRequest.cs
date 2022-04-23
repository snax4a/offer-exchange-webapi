using FSH.WebApi.Application.Comparison.DTOs;

namespace FSH.WebApi.Application.Comparison;

public class GetProductOffersRequest : IRequest<IEnumerable<InquiryProductOfferDto>>
{
    public Guid InquiryProductId { get; set; }
    public bool WithReplacements { get; set; } = true;
    public bool OnlyWithFreebies { get; set; }
    public ProductOffersOrder OrderBy { get; set; }

    public GetProductOffersRequest(
        Guid inquiryProductId,
        bool withReplacements,
        bool onlyWithFreebies,
        ProductOffersOrder orderBy)
    {
        InquiryProductId = inquiryProductId;
        WithReplacements = withReplacements;
        OnlyWithFreebies = onlyWithFreebies;
        OrderBy = orderBy;
    }
}

public class GetProductOffersRequestHandler : IRequestHandler<GetProductOffersRequest, IEnumerable<InquiryProductOfferDto>>
{
    private readonly IComparisonRepository _repository;

    public GetProductOffersRequestHandler(IComparisonRepository repository) => _repository = repository;

    public async Task<IEnumerable<InquiryProductOfferDto>> Handle(GetProductOffersRequest request, CancellationToken ct)
    {
        return await _repository.GetOffersForInquiryProductAsync(
            request.InquiryProductId,
            request.WithReplacements,
            request.OnlyWithFreebies,
            request.OrderBy,
            ct);
    }
}