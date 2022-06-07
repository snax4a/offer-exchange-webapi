using FSH.WebApi.Application.Exchange.Addresses.Specifications;

namespace FSH.WebApi.Application.Exchange.Addresses;

public class UpdateUserAddressRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string CountryCode { get; set; } = default!;
    public string CountrySubdivisionName { get; set; } = default!;
    public string Line1 { get; set; } = default!;
    public string? Line2 { get; set; }
    public string PostalCode { get; set; } = default!;
    public string Locality { get; set; } = default!;
}

public class UpdateUserAddressRequestValidator : CustomValidator<UpdateUserAddressRequest>
{
    public UpdateUserAddressRequestValidator(
        ICurrentUser currentUser,
        IReadRepository<Country> countryRepo,
        IReadRepository<UserAddress> addressRepo,
        IStringLocalizer<CreateUserAddressRequestValidator> localizer)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(ua => ua.Name)
            .NotEmpty()
            .Length(1, 100)
            .MustAsync(async (req, name, ct) => await addressRepo.GetBySpecAsync(new UserAddressByNameAndNotIdSpec(name, req.Id, currentUser.GetUserId()), ct) is null)
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

public class UpdateUserAddressRequestHandler : IRequestHandler<UpdateUserAddressRequest, Guid>
{
    private readonly ICurrentUser _currentUser;
    private readonly IRepositoryWithEvents<Address> _addressRepo;
    private readonly IRepositoryWithEvents<UserAddress> _userAddressRepo;
    private readonly IStringLocalizer<UpdateUserAddressRequestHandler> _localizer;

    public UpdateUserAddressRequestHandler(
        ICurrentUser currentUser,
        IRepositoryWithEvents<Address> addressRepo,
        IRepositoryWithEvents<UserAddress> userAddressRepo,
        IStringLocalizer<UpdateUserAddressRequestHandler> localizer)
            => (_currentUser, _addressRepo, _userAddressRepo, _localizer) = (currentUser, addressRepo, userAddressRepo, localizer);

    public async Task<Guid> Handle(UpdateUserAddressRequest req, CancellationToken cancellationToken)
    {
        var spec = new UserAddressByIdSpec(req.Id, _currentUser.GetUserId());
        var userAddress = await _userAddressRepo.GetBySpecAsync(spec, cancellationToken);

        _ = userAddress ?? throw new NotFoundException(_localizer["address.notfound"]);

        var address = new Address(req.CountryCode, req.CountrySubdivisionName, req.Line1, req.Line2, req.PostalCode, req.Locality);

        if (userAddress.Address.Equals(address))
        {
            userAddress.Update(req.Name, null);
        }
        else
        {
            // Add new address record to the database
            await _addressRepo.AddAsync(address, cancellationToken);

            // Link new address to user address entity
            userAddress.Update(req.Name, address);
        }

        await _userAddressRepo.UpdateAsync(userAddress, cancellationToken);

        return req.Id;
    }
}