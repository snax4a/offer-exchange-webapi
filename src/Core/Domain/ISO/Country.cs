using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace FSH.WebApi.Domain.Exchange;

public class Country : IAggregateRoot
{
    public string Alpha2Code { get; private set; } = default!;
    public string Alpha3Code { get; private set; } = default!;
    public string NumericCode { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string? CallingCodes { get; private set; }
    public string? CurrencyCode { get; private set; }
    public string? CurrencyName { get; private set; }
    public string? CurrencySymbol { get; private set; }
    public string? Capital { get; private set; }
    public string? LanguageCodes { get; private set; }
    public ICollection<CountrySubdivision> Subdivisions { get; private set; } = new List<CountrySubdivision>();

    [NotMapped]
    public List<DomainEvent> DomainEvents { get; } = new();

    private Country()
    {
        // Required by ORM
    }

    public Country(
        string alpha2Code,
        string alpha3Code,
        string numericCode,
        string name,
        string? callingCodes,
        string? currencyCode,
        string? currencyName,
        string? currencySymbol,
        string? capital,
        string? languageCodes)
    {
        if (alpha2Code.Length != 2) throw new ArgumentException(nameof(alpha2Code));
        if (alpha3Code.Length != 3) throw new ArgumentException(nameof(alpha3Code));
        if (Regex.Match(numericCode, @"\d{3}$").Success) throw new ArgumentException(nameof(numericCode));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (currencyCode is not null && currencyCode.Length != 3) throw new ArgumentException(nameof(currencyCode));
        if (currencyName?.Length <= 3) throw new ArgumentException(nameof(currencyName));
        if (currencySymbol?.Length == 0) throw new ArgumentException(nameof(currencySymbol));
        if (languageCodes?.Length == 0) throw new ArgumentException(nameof(languageCodes));

        Alpha2Code = alpha2Code;
        Alpha3Code = alpha3Code;
        NumericCode = numericCode;
        Name = name;
        CallingCodes = callingCodes;
        CurrencyCode = currencyCode;
        Capital = capital;
        LanguageCodes = languageCodes;
    }
}