namespace FSH.WebApi.Domain.Exchange;

public class Trader : AuditableEntity, IAggregateRoot
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public ICollection<TraderGroup> TraderGroups { get; private set; } = new List<TraderGroup>();

    public Trader(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public Trader Update(string? firstName, string? lastName, string? email)
    {
        if (firstName is not null && FirstName?.Equals(firstName) is not true) FirstName = firstName;
        if (lastName is not null && LastName?.Equals(lastName) is not true) LastName = lastName;
        if (email is not null && Email?.Equals(email) is not true) Email = email;
        return this;
    }

    public Trader AddGroup(Group group)
    {
        if (!TraderGroups.Any(x => x.GroupId == group.Id))
        {
            TraderGroups.Add(new TraderGroup(Id, group.Id));
        }

        return this;
    }

    public Trader RemoveGroup(Group group)
    {
        var traderGroup = TraderGroups.FirstOrDefault(x => x.TraderId == Id && x.GroupId == group.Id);
        if (traderGroup is not null) TraderGroups.Remove(traderGroup);
        return this;
    }
}