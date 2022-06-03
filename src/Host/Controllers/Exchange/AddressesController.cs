using FSH.WebApi.Application.Exchange.Addresses;
using FSH.WebApi.Application.Exchange.Addresses.DTOs;

namespace FSH.WebApi.Host.Controllers.Exchange;

public class AddressesController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(ResourceAction.Search, Resource.Addresses)]
    [OpenApiOperation("Search addresses using available filters.", "")]
    public Task<PaginationResponse<UserAddressDto>> SearchAsync(SearchUserAddressesRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(ResourceAction.View, Resource.Addresses)]
    [OpenApiOperation("Get address details.", "")]
    public Task<UserAddressDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetUserAddressDetailsRequest(id));
    }

    [HttpPost]
    [MustHavePermission(ResourceAction.Create, Resource.Addresses)]
    [OpenApiOperation("Create a new address.", "")]
    public Task<Guid> CreateAsync(CreateUserAddressRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(ResourceAction.Create, Resource.Addresses)]
    [OpenApiOperation("Create a new address.", "")]
    public async Task<ActionResult<Guid>> CreateAsync(UpdateUserAddressRequest request, Guid id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(ResourceAction.Delete, Resource.Addresses)]
    [OpenApiOperation("Delete address.", "")]
    public Task<Guid> DeleteAsync(Guid id)
    {
        return Mediator.Send(new DeleteUserAddressRequest(id));
    }

    [HttpPost("countries/search")]
    [OpenApiOperation("Search addresses using available filters.", "")]
    public Task<PaginationResponse<CountryDto>> GetCountriesAsync(SearchCountriesRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("countries/{code}")]
    [MustHavePermission(ResourceAction.View, Resource.Addresses)]
    [OpenApiOperation("Get country details.", "")]
    public Task<CountryDetailsDto> GetAsync(string code)
    {
        return Mediator.Send(new GetCountryDetailsRequest(code));
    }
}