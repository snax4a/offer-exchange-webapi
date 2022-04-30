using FSH.WebApi.Application.Comparison.DTOs;

namespace FSH.WebApi.Application.Comparison;

public class GetTheBestOffersForInquiryRequest : IRequest<IEnumerable<InquiryProductOfferDto>>
{
    public Guid InquiryId { get; set; }
    public bool WithReplacements { get; set; } = true;
    public ComparisonProductSelection ProductSelection { get; set; }
    public ComparisonDecisiveParameter DecisiveParameter { get; set; }
    public DateOnly? MaxDeliveryDate { get; set; }
    public IList<Guid>? ProductIds { get; set; }
}

public class GetTheBestOffersForInquiryRequestValidator : CustomValidator<GetTheBestOffersForInquiryRequest>
{
    public GetTheBestOffersForInquiryRequestValidator()
    {
        RuleFor(r => r.ProductSelection).NotNull();
        RuleFor(r => r.DecisiveParameter).NotNull();

        RuleFor(r => r.ProductIds)
            .Must(ids => ids?.Count > 0)
            .When(r => r.ProductSelection == ComparisonProductSelection.Selected)
            .WithMessage("At least one productId is required.");
    }
}

public class GetTheBestOffersForInquiryRequestHandler : IRequestHandler<GetTheBestOffersForInquiryRequest, IEnumerable<InquiryProductOfferDto>>
{
    private readonly IComparisonRepository _repository;

    public GetTheBestOffersForInquiryRequestHandler(IComparisonRepository repository) => _repository = repository;

    public async Task<IEnumerable<InquiryProductOfferDto>> Handle(GetTheBestOffersForInquiryRequest request, CancellationToken ct)
    {
        if (request.ProductSelection == ComparisonProductSelection.Selected)
        {
            return await _repository.GetTheBestOffersForSelectedProductsFromInquiryAsync(
                request.InquiryId,
                request.ProductIds!,
                request.WithReplacements,
                request.DecisiveParameter,
                request.MaxDeliveryDate,
                ct);
        }
        else
        {
            return await _repository.GetTheBestOffersForAllProductsFromInquiryAsync(
                request.InquiryId,
                request.WithReplacements,
                request.DecisiveParameter,
                request.MaxDeliveryDate,
                ct);
        }
    }
}