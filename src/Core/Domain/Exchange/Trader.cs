namespace FSH.WebApi.Domain.Exchange;

public class Trader : AuditableEntity, IAggregateRoot
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public ICollection<TraderGroup> TraderGroups { get; private set; } = new List<TraderGroup>();
    public ICollection<InquiryRecipient> InquiryRecipients { get; private set; } = new List<InquiryRecipient>();
    public ICollection<Offer> Offers { get; private set; } = new List<Offer>();

    public Trader(string firstName, string lastName, string email)
    {
        if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentNullException(nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentNullException(nameof(lastName));
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));

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

    public Trader AddGroup(Guid groupId)
    {
        if (!TraderGroups.Any(x => x.GroupId == groupId))
        {
            TraderGroups.Add(new TraderGroup(Id, groupId));
        }

        return this;
    }

    public Trader RemoveGroup(Guid groupId)
    {
        var traderGroup = TraderGroups.FirstOrDefault(x => x.TraderId == Id && x.GroupId == groupId);
        if (traderGroup is not null) TraderGroups.Remove(traderGroup);
        return this;
    }

    public Trader ClearGroups()
    {
        TraderGroups.Clear();
        return this;
    }
}