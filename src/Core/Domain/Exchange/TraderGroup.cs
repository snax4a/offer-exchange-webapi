namespace FSH.WebApi.Domain.Exchange;

public class TraderGroup
{
    public Guid TraderId { get; private set; }
    public Guid GroupId { get; private set; }
    public virtual Trader Trader { get; private set; } = default!;
    public virtual Group Group { get; private set; } = default!;

    public TraderGroup(Guid traderId, Guid groupId)
    {
        TraderId = traderId;
        GroupId = groupId;
    }
}