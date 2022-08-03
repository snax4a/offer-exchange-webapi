namespace FSH.WebApi.Domain.Billing;

public class Customer : BaseEntity, IAggregateRoot
{
    public Guid UserId { get; private set; }
    public string StripeCustomerId { get; private set; }
    public string? CurrentSubscriptionId { get; private set; }
    public virtual StripeSubscription? CurrentSubscription { get; private set; }
    public BillingPlan BillingPlan { get; private set; }
    public short MonthlyNumberOfInquiriesSent { get; private set; }
    public ICollection<StripeSubscription> Subscriptions { get; private set; }

    public Customer(Guid userId, string stripeCustomerId)
    {
        if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));
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

    public Customer SetCurrentSubscription(string subscriptionId)
    {
        CurrentSubscriptionId = subscriptionId;
        return this;
    }

    public Customer IncrementMonthlyNumberOfInquiriesSent()
    {
        MonthlyNumberOfInquiriesSent++;
        return this;
    }

    public Customer DecrementMonthlyNumberOfInquiriesSent()
    {
        MonthlyNumberOfInquiriesSent--;
        return this;
    }

    public Customer SetMonthlyNumberOfInquiriesSent(short newValue)
    {
        MonthlyNumberOfInquiriesSent = newValue;
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