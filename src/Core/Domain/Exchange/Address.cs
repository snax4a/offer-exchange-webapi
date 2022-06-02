namespace FSH.WebApi.Domain.Exchange;

public class Address : BaseEntity
{
    public string CountryCode { get; private set; } = default!;
    public string CountrySubdivisionName { get; private set; } = default!; // State or province
    public string Line1 { get; private set; } = default!;
    public string? Line2 { get; private set; }
    public string PostalCode { get; private set; } = default!;
    public string Locality { get; private set; } = default!;

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
}