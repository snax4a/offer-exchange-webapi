using FSH.WebApi.Domain.Billing;

namespace FSH.WebApi.Application.Exchange.Billing.Customers.DTOs;

public class CustomerDto : IDto
{
    public string StripeCustomerId { get; set; } = default!;
    public short MonthlyNumberOfInquiriesSent { get; set; }
    public BillingPlan BillingPlan { get; set; }
}