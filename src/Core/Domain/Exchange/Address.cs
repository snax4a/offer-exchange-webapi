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

    public virtual Country Country { get; private set; }

    private Address()
    {
        // Required by ORM
    }

    public Address(string countryCode, string countrySubdivisionName, string line1, string? line2, string postalCode, string locality)
    {
        if (string.IsNullOrWhiteSpace(countryCode)) throw new ArgumentNullException(nameof(countryCode));
        if (string.IsNullOrWhiteSpace(countrySubdivisionName)) throw new ArgumentNullException(nameof(countrySubdivisionName));
        if (string.IsNullOrWhiteSpace(line1)) throw new ArgumentNullException(nameof(line1));
        if (string.IsNullOrWhiteSpace(postalCode)) throw new ArgumentNullException(nameof(postalCode));
        if (string.IsNullOrWhiteSpace(locality)) throw new ArgumentNullException(nameof(locality));

        CountryCode = countryCode;
        CountrySubdivisionName = countrySubdivisionName;
        Line1 = line1;
        Line2 = line2;
        PostalCode = postalCode;
        Locality = locality;
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