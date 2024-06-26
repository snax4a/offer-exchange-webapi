using FSH.WebApi.Core.Shared.Extensions;

namespace FSH.WebApi.Domain.Exchange;

public class Group : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; }
    public ColorName Color { get; private set; }
    public ICollection<TraderGroup> TraderGroups { get; private set; } = new List<TraderGroup>();

    public Group(string name, ColorName color)
    {
        string strippedName = name.StripHtml();
        if (string.IsNullOrWhiteSpace(strippedName)) throw new ArgumentNullException(nameof(name));

        Name = strippedName;
        Color = color;
    }

    public Group Update(string? name, ColorName? color)
    {
        string? strippedName = name?.StripHtml();
        if (strippedName is not null && Name?.Equals(strippedName) is not true) Name = strippedName;
        if (color is not null && Color != color) Color = (ColorName)color;
        return this;
    }

    public Group AddTrader(Trader trader)
    {
        if (!TraderGroups.Any(x => x.TraderId == trader.Id))
        {
            TraderGroups.Add(new TraderGroup(trader.Id, Id));
        }

        return this;
    }

    public Group RemoveTrader(Trader trader)
    {
        var traderGroup = TraderGroups.FirstOrDefault(x => x.TraderId == trader.Id && x.GroupId == Id);
        if (traderGroup is not null) TraderGroups.Remove(traderGroup);
        return this;
    }
}