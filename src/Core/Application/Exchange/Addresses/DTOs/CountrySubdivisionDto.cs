namespace FSH.WebApi.Application.Exchange.Addresses.DTOs;

public class CountrySubdivisionDto : IDto
{
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
}