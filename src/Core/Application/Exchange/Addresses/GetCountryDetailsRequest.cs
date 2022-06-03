using FSH.WebApi.Application.Exchange.Addresses.DTOs;
using FSH.WebApi.Application.Exchange.Addresses.Specifications;

namespace FSH.WebApi.Application.Exchange.Addresses;

public class GetCountryDetailsRequest : IRequest<CountryDetailsDto>
{
    public string Alpha2Code { get; set; }

    public GetCountryDetailsRequest(string alpha2Code) => Alpha2Code = alpha2Code;
}

public class GetCountryDetailsRequestHandler : IRequestHandler<GetCountryDetailsRequest, CountryDetailsDto>
{
    private readonly IRepository<Country> _repository;
    private readonly IStringLocalizer<GetCountryDetailsRequestHandler> _localizer;

    public GetCountryDetailsRequestHandler(
        IRepository<Country> repository,
        IStringLocalizer<GetCountryDetailsRequestHandler> localizer)
    {
        (_repository, _localizer) = (repository, localizer);
    }

    public async Task<CountryDetailsDto> Handle(GetCountryDetailsRequest request, CancellationToken cancellationToken)
    {
        ISpecification<Country, CountryDetailsDto> spec = new CountryDetailsSpec(request.Alpha2Code);
        var countryDetails = await _repository.GetBySpecAsync(spec, cancellationToken);

        if (countryDetails is not null) return countryDetails;

        throw new NotFoundException(_localizer["country.notfound"]);
    }
}