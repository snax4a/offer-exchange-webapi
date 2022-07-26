namespace FSH.WebApi.Application.Exchange.Billing.Stripe.DTOs;

public class StripeCheckoutSessionDto
{
    public string Id { get; set; } = default!;
    public string Mode { get; set; } = default!;
    public string Status { get; set; } = default!;
    public string PaymentStatus { get; set; } = default!;
    public long? AmountSubtotal { get; set; }
    public long? AmountTotal { get; set; }
    public string Currency { get; set; } = default!;
    public string CustomerEmail { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
    public bool Livemode { get; set; }
    public string Url { get; set; } = default!;
}