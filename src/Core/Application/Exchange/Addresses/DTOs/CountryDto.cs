namespace FSH.WebApi.Application.Exchange.Addresses.DTOs;

public class CountryDto : IDto
{
    public string Alpha2Code { get; set; } = default!;
    public string Name { get; set; } = default!;
}