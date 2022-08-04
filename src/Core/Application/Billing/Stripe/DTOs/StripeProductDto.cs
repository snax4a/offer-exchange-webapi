namespace FSH.WebApi.Application.Exchange.Billing.Stripe.DTOs;

public class StripeProductDto
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string Metadata { get; set; } = default!;
}