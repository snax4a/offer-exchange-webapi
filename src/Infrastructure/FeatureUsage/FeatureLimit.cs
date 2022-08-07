using FSH.WebApi.Core.Shared.FeatureUsage;

namespace FSH.WebApi.Infrastructure.FeatureUsage;

public class FeatureLimit
{
    public AppFeatureIds FeatureId { get; set; }
    public LimitType Type { get; set; }
    public short Value { get; set; }
}