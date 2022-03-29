namespace FSH.WebApi.Domain.Exchange;

public record DeliveryCost
{
    public DeliveryCostType Type { get; private set; }
    public long GrossPrice { get; private set; }
    public string? Description { get; private set; }

    public DeliveryCost(DeliveryCostType type, long grossPrice, string? description)
    {
        if (type is DeliveryCostType.Fixed && !string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Fixed cost cannot have a description", nameof(description));
        }

        if (type is DeliveryCostType.Variable && string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Variable cost must have a description", nameof(description));
        }

        Type = type;
        GrossPrice = grossPrice;
        Description = description;
    }
}