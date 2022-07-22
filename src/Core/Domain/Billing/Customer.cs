namespace FSH.WebApi.Domain.Billing;

public class Customer : BaseEntity, IAggregateRoot
{
    public string UserId { get; private set; }
    public string StripeCustomerId { get; private set; }
    public BillingPlan BillingPlan { get; private set; }
    public short MonthlyNumberOfInquiriesSent { get; private set; }
    public ICollection<StripeSubscription> Subscriptions { get; private set; }

    public Customer(string userId, string stripeCustomerId)
    {
        if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentNullException(nameof(userId));
        if (string.IsNullOrWhiteSpace(stripeCustomerId)) throw new ArgumentNullException(nameof(stripeCustomerId));

        UserId = userId;
        StripeCustomerId = stripeCustomerId;
        BillingPlan = BillingPlan.Free;
        MonthlyNumberOfInquiriesSent = 0;
        Subscriptions = new List<StripeSubscription>();
    }

    public Customer SetBillingPlan(BillingPlan billingPlan)
    {
        BillingPlan = billingPlan;
        return this;
    }

    public Customer IncreaseMonthlyNumberOfInquiriesSent()
    {
        MonthlyNumberOfInquiriesSent++;
        return this;
    }

    public Customer ResetMonthlyNumberOfInquiriesSent()
    {
        MonthlyNumberOfInquiriesSent = 0;
        return this;
    }

    public Customer AddSubscription(StripeSubscription subscription)
    {
        if (!Subscriptions.Any(x => x.Id == subscription.Id))
        {
            Subscriptions.Add(subscription);
        }

        return this;
    }
}