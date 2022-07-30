using FSH.WebApi.Domain.Billing;

namespace FSH.WebApi.Application.Exchange.Billing.Customers.DTOs;

public class CustomerDto : IDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = default!;
    public string StripeCustomerId { get; set; } = default!;
    public BillingPlan BillingPlan { get; set; }
    public short MonthlyNumberOfInquiriesSent { get; set; }
}