namespace FSH.WebApi.Application.Exchange.Traders;

public class CreateTraderRequest : IRequest<Guid>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
}

public class CreateTraderRequestValidator : CustomValidator<CreateTraderRequest>
{
    public CreateTraderRequestValidator(IReadRepository<Trader> repository, ICurrentUser currentUser, IStringLocalizer<CreateTraderRequestValidator> localizer)
    {
        RuleFor(t => t.FirstName).NotEmpty().MinimumLength(3).MaximumLength(20);
        RuleFor(t => t.LastName).NotEmpty().MinimumLength(3).MaximumLength(20);

        RuleFor(t => t.Email)
            .NotEmpty()
            .EmailAddress()
                .WithMessage(localizer["email.invalid"])
            .MustAsync(async (email, ct) => await repository.GetBySpecAsync(new TraderByEmailSpec(email, currentUser.GetUserId()), ct) is null)
                .WithMessage((_, email) => string.Format(localizer["trader.alreadyexists"], email));
    }
}

public class CreateTraderRequestHandler : IRequestHandler<CreateTraderRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Trader> _repository;

    public CreateTraderRequestHandler(IRepositoryWithEvents<Trader> repository) => _repository = repository;

    public async Task<Guid> Handle(CreateTraderRequest request, CancellationToken cancellationToken)
    {
        var trader = new Trader(request.FirstName, request.LastName, request.Email);

        await _repository.AddAsync(trader, cancellationToken);

        return trader.Id;
    }
}