using FSH.WebApi.Application.Exchange.Addresses.DTOs;
using FSH.WebApi.Application.Exchange.Addresses.Specifications;

namespace FSH.WebApi.Application.Exchange.Addresses;

public class SearchCountriesRequest : PaginationFilter, IRequest<PaginationResponse<CountryDto>>
{
}

public class SearchCountriesRequestHandler : IRequestHandler<SearchCountriesRequest, PaginationResponse<CountryDto>>
{
    private readonly IReadRepository<Country> _repository;

    public SearchCountriesRequestHandler(IReadRepository<Country> repository)
        => _repository = repository;

    public async Task<PaginationResponse<CountryDto>> Handle(SearchCountriesRequest request, CancellationToken cancellationToken)
    {
        var spec = new SearchCountriesSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);
    }
}