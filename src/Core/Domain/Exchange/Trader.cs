using FSH.WebApi.Core.Shared.Extensions;

namespace FSH.WebApi.Domain.Exchange;

public class Trader : AuditableEntity, IAggregateRoot
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string? CompanyName { get; private set; }
    public ICollection<TraderGroup> TraderGroups { get; private set; } = new List<TraderGroup>();
    public ICollection<InquiryRecipient> InquiryRecipients { get; private set; } = new List<InquiryRecipient>();
    public ICollection<Offer> Offers { get; private set; } = new List<Offer>();

    public Trader(string firstName, string lastName, string email, string? companyName)
    {
        string strippedFirstName = firstName.StripHtml();
        string strippedLastName = lastName.StripHtml();
        string strippedEmail = email.StripHtml();
        string? strippedCompanyName = companyName?.StripHtml();

        if (string.IsNullOrWhiteSpace(strippedFirstName)) throw new ArgumentNullException(nameof(firstName));
        if (string.IsNullOrWhiteSpace(strippedLastName)) throw new ArgumentNullException(nameof(lastName));
        if (string.IsNullOrWhiteSpace(strippedEmail)) throw new ArgumentNullException(nameof(email));
        if (strippedCompanyName?.Length == 0) throw new ArgumentException(nameof(companyName));

        FirstName = strippedFirstName;
        LastName = strippedLastName;
        Email = strippedEmail;
        CompanyName = strippedCompanyName;
    }

    public Trader Update(string? firstName, string? lastName, string? email, string? companyName)
    {
        string? strippedFirstName = firstName?.StripHtml();
        string? strippedLastName = lastName?.StripHtml();
        string? strippedEmail = email?.StripHtml();
        string? strippedCompanyName = companyName?.StripHtml();

        if (strippedFirstName is not null && FirstName?.Equals(firstName) is not true) FirstName = strippedFirstName;
        if (strippedLastName is not null && LastName?.Equals(lastName) is not true) LastName = strippedLastName;
        if (strippedEmail is not null && Email?.Equals(email) is not true) Email = strippedEmail;
        if (CompanyName?.Equals(strippedCompanyName) is not true) CompanyName = strippedCompanyName;
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