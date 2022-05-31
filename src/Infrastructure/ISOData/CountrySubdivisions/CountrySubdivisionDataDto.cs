using FSH.WebApi.Application.Common.Interfaces;
using Newtonsoft.Json;

namespace FSH.WebApi.Infrastructure.ISOData.CountrySubdivisions;

public class CountrySubdivisionDataDto : IDto
{
    [JsonProperty("code")]
    public string Code { get; set; } = default!;

    [JsonProperty("subdivision_name")]
    public string Name { get; set; } = default!;

    [JsonProperty("country_code")]
    public string CountryAlpha2Code { get; set; } = default!;
}