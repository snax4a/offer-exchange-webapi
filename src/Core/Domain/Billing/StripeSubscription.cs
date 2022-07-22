namespace FSH.WebApi.Domain.Billing;

public class StripeSubscription : BaseEntity<string>
{
    public string CustomerId { get; private set; }
    public virtual Customer Customer { get; private set; } = default!;
    public string Status { get; private set; }
    public string PriceId { get; private set; }

    // Summary:
    //     If the subscription has been canceled with the at_period_end flag set to true,
    //     cancel_at_period_end on the subscription will be true. You can use this attribute
    //     to determine whether a subscription that has a status of active is scheduled
    //     to be canceled at the end of the current period.
    public bool CancelAtPeriodEnd { get; private set; }

    // Summary:
    //     A date in the future at which the subscription will automatically get canceled.
    public DateTime? CancelAt { get; private set; }

    // Summary:
    //     If the subscription has been canceled, the date of that cancellation. If the
    //     subscription was canceled with cancel_at_period_end, canceled_at will reflect
    //     the time of the most recent update request, not the end of the subscription period
    //     when the subscription is automatically moved to a canceled state.
    public DateTime? CanceledAt { get; private set; }

    // Summary:
    //     Either charge_automatically, or send_invoice. When charging automatically, Stripe
    //     will attempt to pay this subscription at the end of the cycle using the default
    //     source attached to the customer. When sending an invoice, Stripe will email your
    //     customer an invoice with payment instructions. One of: charge_automatically,
    //     or send_invoice.
    public string CollectionMethod { get; private set; }

    // Summary:
    //     Time at which the object was created. Measured in seconds since the Unix epoch.
    public DateTime Created { get; private set; }

    // Summary:
    //     Three-letter ISO currency code, in lowercase. Must be a supported currency.
    public string Currency { get; private set; }

    // Summary:
    //     End of the current period that the subscription has been invoiced for. At the
    //     end of this period, a new invoice will be created.
    public DateTime CurrentPeriodEnd { get; private set; }

    // Summary:
    //     Start of the current period that the subscription has been invoiced for.
    public DateTime CurrentPeriodStart { get; private set; }

    // Summary:
    //     Date when the subscription was first created. The date might differ from the
    //     created date due to backdating.
    public DateTime StartDate { get; set; }

    // Summary:
    //     If the subscription has ended, the date the subscription ended.
    public DateTime? EndedAt { get; private set; }

    // Summary:
    //     If the subscription has a trial, the beginning of that trial.
    public DateTime? TrialStart { get; private set; }

    // Summary:
    //     If the subscription has a trial, the end of that trial.
    public DateTime? TrialEnd { get; private set; }

    // Summary:
    //     Has the value true if the object exists in live mode or the value false if the
    //     object exists in test mode.
    public bool Livemode { get; private set; }

    public StripeSubscription(
        string id,
        string customerId,
        string status,
        string priceId,
        bool cancelAtPeriodEnd,
        DateTime? cancelAt,
        DateTime? canceledAt,
        string collectionMethod,
        DateTime created,
        string currency,
        DateTime currentPeriodEnd,
        DateTime currentPeriodStart,
        DateTime startDate,
        DateTime? endedAt,
        DateTime? trialStart,
        DateTime? trialEnd,
        bool livemode)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));
        if (string.IsNullOrWhiteSpace(customerId)) throw new ArgumentNullException(nameof(customerId));
        if (string.IsNullOrWhiteSpace(status)) throw new ArgumentNullException(nameof(status));
        if (string.IsNullOrWhiteSpace(priceId)) throw new ArgumentNullException(nameof(priceId));
        if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentNullException(nameof(currency));
        if (currentPeriodEnd == default) throw new ArgumentNullException(nameof(currentPeriodEnd));
        if (currentPeriodStart == default) throw new ArgumentNullException(nameof(currentPeriodStart));
        if (startDate == default) throw new ArgumentNullException(nameof(startDate));

        Id = id;
        CustomerId = customerId;
        Status = status;
        PriceId = priceId;
        CancelAtPeriodEnd = cancelAtPeriodEnd;
        CancelAt = cancelAt;
        CanceledAt = canceledAt;
        CollectionMethod = collectionMethod;
        Created = created;
        Currency = currency;
        CurrentPeriodEnd = currentPeriodEnd;
        CurrentPeriodStart = currentPeriodStart;
        StartDate = startDate;
        EndedAt = endedAt;
        TrialStart = trialStart;
        TrialEnd = trialEnd;
        Livemode = livemode;
    }

    public StripeSubscription Update(
        string status,
        bool cancelAtPeriodEnd,
        DateTime? cancelAt,
        DateTime? canceledAt,
        DateTime currentPeriodEnd,
        DateTime currentPeriodStart,
        DateTime startDate,
        DateTime? endedAt,
        DateTime? trialStart,
        DateTime? trialEnd)
    {
        if (currentPeriodEnd == default) throw new ArgumentNullException(nameof(currentPeriodEnd));
        if (currentPeriodStart == default) throw new ArgumentNullException(nameof(currentPeriodStart));
        if (startDate == default) throw new ArgumentNullException(nameof(startDate));

        Status = status;
        CancelAtPeriodEnd = cancelAtPeriodEnd;
        CancelAt = cancelAt;
        CanceledAt = canceledAt;
        CurrentPeriodEnd = currentPeriodEnd;
        CurrentPeriodStart = currentPeriodStart;
        StartDate = startDate;
        EndedAt = endedAt;
        TrialStart = trialStart;
        TrialEnd = trialEnd;

        return this;
    }
}