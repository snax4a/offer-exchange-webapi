namespace FSH.WebApi.Application.Exchange.Billing.Stripe.DTOs;

public class StripePriceDto
{
    public long? UnitAmount { get; set; }
    public decimal? UnitAmountDecimal { get; set; }
    public string Currency { get; set; } = default!;
    public string Interval { get; set; } = default!;
    public long? TrialPeriodDays { get; set; }
}