using FSH.WebApi.Core.Shared.FeatureUsage;

namespace FSH.WebApi.Application.Exchange.Billing.Customers.DTOs;

public class FeatureUsageDetailsDto : IDto
{
    public AppFeatureIds FeatureId { get; set; }
    public short? Usage { get; set; }
    public short Limit { get; set; }
    public LimitType LimitType { get; set; }
}