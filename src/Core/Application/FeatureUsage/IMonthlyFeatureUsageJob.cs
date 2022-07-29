using System.ComponentModel;

namespace FSH.WebApi.Application.FeatureUsage;

public interface IMonthlyFeatureUsageJob : IScopedService
{
    [DisplayName("Reset all customers monthly feature usage data.")]
    Task ResetAllCustomersUsageDataAsync();
}