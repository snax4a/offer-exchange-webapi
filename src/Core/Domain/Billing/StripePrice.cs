namespace FSH.WebApi.Domain.Billing;

public class StripePrice : BaseEntity<string>, IAggregateRoot
{
    public string ProductId { get; private set; }
    public virtual StripeProduct Product { get; set; } = default!;
    public string Type { get; private set; }
    public string? Description { get; private set; }
    public long? UnitAmount { get; private set; }
    public decimal? UnitAmountDecimal { get; private set; }
    public string Currency { get; private set; }
    public string TaxBehavior { get; private set; }
    public string Interval { get; private set; }
    public long IntervalCount { get; private set; }
    public long? TrialPeriodDays { get; private set; }
    public bool IsActive { get; private set; }
    public bool Livemode { get; private set; }
    public string Metadata { get; private set; }

    public StripePrice(
        string id,
        string productId,
        string type,
        string? description,
        long? unitAmount,
        decimal? unitAmountDecimal,
        string currency,
        string taxBehavior,
        string interval,
        long intervalCount,
        long? trialPeriodDays,
        bool isActive,
        bool livemode,
        string metadata)
    {
        if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));
        if (string.IsNullOrWhiteSpace(productId)) throw new ArgumentNullException(nameof(productId));
        if (string.IsNullOrWhiteSpace(type)) throw new ArgumentNullException(nameof(type));
        if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentNullException(nameof(currency));
        if (string.IsNullOrWhiteSpace(taxBehavior)) throw new ArgumentNullException(nameof(taxBehavior));
        if (string.IsNullOrWhiteSpace(interval)) throw new ArgumentNullException(nameof(interval));
        if (intervalCount <= 0) throw new ArgumentOutOfRangeException(nameof(intervalCount));
        if (trialPeriodDays < 0) throw new ArgumentOutOfRangeException(nameof(trialPeriodDays));
        if (unitAmount < 0) throw new ArgumentOutOfRangeException(nameof(unitAmount));
        if (unitAmountDecimal < 0) throw new ArgumentOutOfRangeException(nameof(unitAmountDecimal));
        if (string.IsNullOrWhiteSpace(metadata)) throw new ArgumentNullException(nameof(metadata));

        Id = id;
        ProductId = productId;
        Type = type;
        Description = description;
        UnitAmount = unitAmount;
        UnitAmountDecimal = unitAmountDecimal;
        Currency = currency;
        TaxBehavior = taxBehavior;
        Interval = interval;
        IntervalCount = intervalCount;
        TrialPeriodDays = trialPeriodDays;
        IsActive = isActive;
        Livemode = livemode;
        Metadata = metadata;
    }

    public StripePrice Update(
        string type,
        string? description,
        long? unitAmount,
        decimal? unitAmountDecimal,
        string currency,
        string taxBehavior,
        string interval,
        long intervalCount,
        long? trialPeriodDays,
        bool isActive,
        bool livemode,
        string metadata)
    {
        if (string.IsNullOrWhiteSpace(type)) throw new ArgumentNullException(nameof(type));
        if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentNullException(nameof(currency));
        if (string.IsNullOrWhiteSpace(taxBehavior)) throw new ArgumentNullException(nameof(taxBehavior));
        if (string.IsNullOrWhiteSpace(interval)) throw new ArgumentNullException(nameof(interval));
        if (intervalCount <= 0) throw new ArgumentOutOfRangeException(nameof(intervalCount));
        if (trialPeriodDays < 0) throw new ArgumentOutOfRangeException(nameof(trialPeriodDays));
        if (unitAmount < 0) throw new ArgumentOutOfRangeException(nameof(unitAmount));
        if (unitAmountDecimal < 0) throw new ArgumentOutOfRangeException(nameof(unitAmountDecimal));
        if (string.IsNullOrWhiteSpace(metadata)) throw new ArgumentNullException(nameof(metadata));

        Type = type;
        Description = description;
        UnitAmount = unitAmount;
        UnitAmountDecimal = unitAmountDecimal;
        Currency = currency;
        TaxBehavior = taxBehavior;
        Interval = interval;
        IntervalCount = intervalCount;
        TrialPeriodDays = trialPeriodDays;
        IsActive = isActive;
        Livemode = livemode;
        Metadata = metadata;

        return this;
    }
}