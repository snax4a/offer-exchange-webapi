namespace FSH.WebApi.Application.Exchange.Billing.Stripe.DTOs;

public class StripeCustomerPortalSessionDto
{
    public string Id { get; set; } = default!;
    public DateTime Created { get; set; }
    public string Customer { get; set; } = default!;
    public bool Livemode { get; set; }
    public string Locale { get; set; } = default!;
    public string ReturnUrl { get; set; } = default!;
    public string Url { get; set; } = default!;
}