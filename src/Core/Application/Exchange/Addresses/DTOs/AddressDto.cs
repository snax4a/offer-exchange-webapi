namespace FSH.WebApi.Application.Exchange.Addresses.DTOs;

public class AddressDto : IDto
{
    public string CountryCode { get; set; } = default!;
    public string CountrySubdivisionName { get; set; } = default!;
    public string Line1 { get; set; } = default!;
    public string? Line2 { get; set; }
    public string PostalCode { get; set; } = default!;
    public string Locality { get; set; } = default!;
}