namespace FSH.WebApi.Domain.Billing;

public class StripeEvent : BaseEntity<string>, IAggregateRoot
{
    public string? AccountId { get; private set; }
    public string? ApiVersion { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ProcessedAt { get; private set; }

    public StripeEvent(string id, string? accountId, string apiVersion, DateTime createdAt)
    {
        Id = id;
        AccountId = accountId;
        ApiVersion = apiVersion;
        CreatedAt = createdAt;
        ProcessedAt = DateTime.UtcNow;
    }
}