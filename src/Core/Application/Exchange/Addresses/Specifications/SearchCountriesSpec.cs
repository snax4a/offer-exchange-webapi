using FSH.WebApi.Application.Exchange.Addresses.DTOs;

namespace FSH.WebApi.Application.Exchange.Addresses.Specifications;

public class SearchCountriesSpec : EntitiesByBaseFilterSpec<Country, CountryDto>
{
    public SearchCountriesSpec(SearchCountriesRequest request)
        : base(request) => Query
            .OrderBy(c => c.Name, !request.HasOrderBy())
            .PaginateBy(request, 250);

}