using FSH.WebApi.Application.Common.Interfaces;
using Newtonsoft.Json;

namespace FSH.WebApi.Infrastructure.ISOData.Countries;

public class CountryDataDto : IDto
{
    [JsonProperty("country_code")]
    public string Alpha2Code { get; set; } = default!;

    [JsonProperty("country_alpha3_code")]
    public string Alpha3Code { get; set; } = default!;

    [JsonProperty("country_numeric_code")]
    public short NumericCode { get; set; } = default!;

    [JsonProperty("country_name")]
    public string Name { get; set; } = default!;

    [JsonProperty("idd_code")]
    public string? CallingCodes { get; set; }

    [JsonProperty("currency_code")]
    public string? CurrencyCode { get; set; }

    [JsonProperty("currency_name")]
    public string? CurrencyName { get; set; }

    [JsonProperty("currency_symbol")]
    public string? CurrencySymbol { get; set; }

    [JsonProperty("capital")]
    public string? Capital { get; set; }

    [JsonProperty("lang_code")]
    public string? LanguageCodes { get; set; }
}