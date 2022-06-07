using FSH.WebApi.Application.Exchange.Addresses.DTOs;
using FSH.WebApi.Application.Exchange.Addresses.Specifications;
using Mapster;

namespace FSH.WebApi.Application.Exchange.Addresses;

public class CreateUserAddressRequest : IRequest<UserAddressDto>
{
    public string Name { get; set; } = default!;
    public string CountryCode { get; set; } = default!;
    public string CountrySubdivisionName { get; set; } = default!;
    public string Line1 { get; set; } = default!;
    public string? Line2 { get; set; }
    public string PostalCode { get; set; } = default!;
    public string Locality { get; set; } = default!;
}

public class CreateUserAddressRequestValidator : CustomValidator<CreateUserAddressRequest>
{
    public CreateUserAddressRequestValidator(
        ICurrentUser currentUser,
        IReadRepository<Country> countryRepo,
        IReadRepository<UserAddress> addressRepo,
        IStringLocalizer<CreateUserAddressRequestValidator> localizer)
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(ua => ua.Name)
            .NotEmpty()
            .Length(1, 100)
            .MustAsync(async (name, ct) => await addressRepo.GetBySpecAsync(new UserAddressByNameSpec(name, currentUser.GetUserId()), ct) is null)
                .WithMessage((_, name) => string.Format(localizer["address.name-already-exists"], name));

        RuleFor(ua => ua.CountryCode)
            .NotEmpty()
            .Length(2)
            .MustAsync(async (code, ct) => await countryRepo.GetByIdAsync(code, ct) is not null)
            .WithMessage((_, name) => string.Format(localizer["country.notfound"], name));

        RuleFor(ua => ua.CountrySubdivisionName).NotEmpty().Length(2, 100);
        RuleFor(ua => ua.Line1).NotEmpty().Length(3, 100);
        RuleFor(ua => ua.Line2).Length(1, 100).Unless(ua => ua.Line2 is null);
        RuleFor(ua => ua.PostalCode).NotEmpty().MaximumLength(12);
        RuleFor(ua => ua.Locality).NotEmpty().Length(3, 60);
    }
}

public class CreateUserAddressRequestHandler : IRequestHandler<CreateUserAddressRequest, UserAddressDto>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<UserAddress> _repository;

    public CreateUserAddressRequestHandler(IRepositoryWithEvents<UserAddress> repository) => _repository = repository;

    public async Task<UserAddressDto> Handle(CreateUserAddressRequest req, CancellationToken cancellationToken)
    {
        var address = new Address(req.CountryCode, req.CountrySubdivisionName, req.Line1, req.Line2, req.PostalCode, req.Locality);
        var userAddress = new UserAddress(req.Name, address);

        await _repository.AddAsync(userAddress, cancellationToken);

        return userAddress.Adapt<UserAddressDto>();
    }
}