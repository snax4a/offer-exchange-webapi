using System.ComponentModel.DataAnnotations;

namespace FSH.WebApi.Infrastructure.PaymentGateways.Stripe;

public class StripeSettings : IValidatableObject
{
    public string SecretKey { get; set; } = string.Empty;
    public string WebhookSecret { get; set; } = string.Empty;
    public string SuccessUrl { get; set; } = string.Empty;
    public string CancelUrl { get; set; } = string.Empty;
    public string PortalReturnUrl { get; set; } = string.Empty;
    public string BasicProductId { get; set; } = string.Empty;
    public string StandardProductId { get; set; } = string.Empty;
    public string EnterpriseProductId { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(SecretKey))
        {
            yield return new ValidationResult(
                $"{nameof(StripeSettings)}.{nameof(SecretKey)} is not configured",
                new[] { nameof(SecretKey) });
        }

        if (string.IsNullOrEmpty(WebhookSecret))
        {
            yield return new ValidationResult(
                $"{nameof(StripeSettings)}.{nameof(WebhookSecret)} is not configured",
                new[] { nameof(WebhookSecret) });
        }

        if (string.IsNullOrEmpty(SuccessUrl))
        {
            yield return new ValidationResult(
                $"{nameof(StripeSettings)}.{nameof(SuccessUrl)} is not configured",
                new[] { nameof(SuccessUrl) });
        }

        if (string.IsNullOrEmpty(CancelUrl))
        {
            yield return new ValidationResult(
                $"{nameof(StripeSettings)}.{nameof(CancelUrl)} is not configured",
                new[] { nameof(CancelUrl) });
        }

        if (string.IsNullOrEmpty(PortalReturnUrl))
        {
            yield return new ValidationResult(
                $"{nameof(StripeSettings)}.{nameof(PortalReturnUrl)} is not configured",
                new[] { nameof(PortalReturnUrl) });
        }

        if (string.IsNullOrEmpty(BasicProductId))
        {
            yield return new ValidationResult(
                $"{nameof(StripeSettings)}.{nameof(BasicProductId)} is not configured",
                new[] { nameof(BasicProductId) });
        }

        if (string.IsNullOrEmpty(StandardProductId))
        {
            yield return new ValidationResult(
                $"{nameof(StripeSettings)}.{nameof(StandardProductId)} is not configured",
                new[] { nameof(StandardProductId) });
        }

        if (string.IsNullOrEmpty(EnterpriseProductId))
        {
            yield return new ValidationResult(
                $"{nameof(StripeSettings)}.{nameof(EnterpriseProductId)} is not configured",
                new[] { nameof(EnterpriseProductId) });
        }
    }
}