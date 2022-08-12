using FSH.WebApi.Core.Shared.Extensions;

namespace FSH.WebApi.Domain.Exchange;

// TODO: In the feature maybe we should make it a value object
// and store in the aggregates as owned types
public class Address : BaseEntity, IAggregateRoot
{
    public string CountryCode { get; private set; } = default!;
    public string CountrySubdivisionName { get; private set; } = default!; // State or province
    public string Line1 { get; private set; } = default!;
    public string? Line2 { get; private set; }
    public string PostalCode { get; private set; } = default!;
    public string Locality { get; private set; } = default!;

    public virtual Country Country { get; private set; } = default!;

    private Address()
    {
        // Required by ORM
    }

    public Address(string countryCode, string countrySubdivisionName, string line1, string? line2, string postalCode, string locality)
    {
        string strippedCountryCode = countryCode.StripHtml();
        string strippedCountrySubdivisionName = countrySubdivisionName.StripHtml();
        string strippedLine1 = line1.StripHtml();
        string? strippedLine2 = line2?.StripHtml();
        string strippedPostalCode = postalCode.StripHtml();
        string strippedLocality = locality.StripHtml();

        if (string.IsNullOrWhiteSpace(strippedCountryCode)) throw new ArgumentNullException(nameof(countryCode));
        if (string.IsNullOrWhiteSpace(strippedCountrySubdivisionName)) throw new ArgumentNullException(nameof(countrySubdivisionName));
        if (string.IsNullOrWhiteSpace(strippedLine1)) throw new ArgumentNullException(nameof(line1));
        if (string.IsNullOrWhiteSpace(strippedPostalCode)) throw new ArgumentNullException(nameof(postalCode));
        if (string.IsNullOrWhiteSpace(strippedLocality)) throw new ArgumentNullException(nameof(locality));

        CountryCode = strippedCountryCode;
        CountrySubdivisionName = strippedCountrySubdivisionName;
        Line1 = strippedLine1;
        Line2 = strippedLine2;
        PostalCode = strippedPostalCode;
        Locality = strippedLocality;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (Address)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }

    public IEnumerable<object> GetEqualityComponents()
    {
        // Using a yield return statement to return each element one at a time
        yield return CountryCode;
        yield return CountrySubdivisionName;
        yield return Line1;
        yield return Line2;
        yield return PostalCode;
        yield return Locality;
    }
}