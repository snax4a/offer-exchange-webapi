using FSH.WebApi.Application.Comparison;
using FSH.WebApi.Application.Comparison.DTOs;

namespace FSH.WebApi.Application.Common.Persistence;

public interface IComparisonRepository : ITransientService
{
    Task<IEnumerable<InquiryProductOfferDto>> GetOffersForInquiryProductAsync(
        Guid productId,
        bool withReplacements,
        bool onlyWithFreebies,
        ProductOffersOrder orderBy,
        CancellationToken ct);
    Task<IEnumerable<InquiryProductOfferDto>> GetTheBestOffersForAllProductsFromInquiryAsync(
        Guid inquiryId,
        bool withReplacements,
        ComparisonDecisiveParameter decisiveParameter,
        CancellationToken ct);
    Task<IEnumerable<InquiryProductOfferDto>> GetTheBestOffersForSelectedProductsFromInquiryAsync(
        Guid inquiryId,
        IList<Guid> productIds,
        bool withReplacements,
        ComparisonDecisiveParameter decisiveParameter,
        CancellationToken ct);
}