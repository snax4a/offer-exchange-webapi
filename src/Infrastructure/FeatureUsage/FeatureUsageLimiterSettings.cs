using System.ComponentModel.DataAnnotations;
using FSH.WebApi.Domain.Billing;

namespace FSH.WebApi.Infrastructure.FeatureUsage;

public class FeatureUsageLimiterSettings : IValidatableObject
{
    public bool EnableLimiter { get; set; } = true;
    public List<Plan> Plans { get; set; } = new List<Plan>();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EnableLimiter && Plans.Count == 0)
        {
            yield return new ValidationResult(
                $"{nameof(FeatureUsageLimiterSettings)}.{nameof(Plans)} is not configured",
                new[] { nameof(Plans) });
        }
    }
}

public class Plan
{
    public BillingPlan PlanId { get; set; }
    public List<FeatureLimit> FeatureLimits { get; set; } = new List<FeatureLimit>();
}