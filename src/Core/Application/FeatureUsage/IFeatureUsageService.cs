using FSH.WebApi.Application.Exchange.Billing.Customers.DTOs;
using FSH.WebApi.Core.Shared.FeatureUsage;

namespace FSH.Webapi.Core.Application.FeatureUsage;

public interface IFeatureUsageService : ITransientService
{
    Task<bool> CanUseFeature(AppFeatureIds featureId, short? valueToCheck = null);
    Task IncrementUsage(AppFeatureIds featureId);
    Task DecrementUsage(AppFeatureIds featureId);
    Task SetUsage(AppFeatureIds featureId, short newValue);
    Task<List<FeatureUsageDetailsDto>> GetFeatureUsageData();
}