namespace FSH.WebApi.Application.Exchange.Billing.Stripe.DTOs;

public class StripeSubscriptionDto
{
    public string Status { get; set; } = default!;
    public string PriceId { get; set; } = default!;
    public bool CancelAtPeriodEnd { get; set; }
    public DateTime? CancelAt { get; set; }
    public DateTime? CanceledAt { get; set; }
    public string Currency { get; set; } = default!;
    public DateTime CurrentPeriodEnd { get; set; }
    public DateTime CurrentPeriodStart { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndedAt { get; set; }
}