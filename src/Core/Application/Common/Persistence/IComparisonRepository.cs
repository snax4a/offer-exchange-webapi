using FSH.WebApi.Application.Comparison.DTOs;

namespace FSH.WebApi.Application.Common.Persistence;

public interface IComparisonRepository : ITransientService
{
    Task<IEnumerable<InquiryProductOfferDto>> GetInquiryProductOffersAsync(Guid productId, Guid userId, CancellationToken ct);
}