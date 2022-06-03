using FSH.WebApi.Application.Exchange.Addresses.DTOs;

namespace FSH.WebApi.Application.Exchange.Addresses.Specifications;

public class CountryDetailsSpec : Specification<Country, CountryDetailsDto>, ISingleResultSpecification
{
    public CountryDetailsSpec(string alpha2Code) =>
        Query.Where(c => c.Alpha2Code == alpha2Code).Include(c => c.Subdivisions);
}