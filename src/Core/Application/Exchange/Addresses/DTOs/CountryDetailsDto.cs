namespace FSH.WebApi.Application.Exchange.Addresses.DTOs;

public class CountryDetailsDto : CountryDto
{
    public string Alpha3Code { get; set; } = default!;
    public string NumericCode { get; set; } = default!;
    public string? CallingCodes { get; set; }
    public string? CurrencyCode { get; set; }
    public string? CurrencyName { get; set; }
    public string? CurrencySymbol { get; set; }
    public string? Capital { get; set; }
    public string? LanguageCodes { get; set; }
    public ICollection<CountrySubdivisionDto> Subdivisions { get; set; } = default!;
}