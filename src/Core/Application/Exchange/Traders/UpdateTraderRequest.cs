namespace FSH.WebApi.Application.Exchange.Traders;

public class UpdateTraderRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
}

public class UpdateTraderRequestValidator : CustomValidator<UpdateTraderRequest>
{
    public UpdateTraderRequestValidator(IRepository<Trader> repository, ICurrentUser currentUser, IStringLocalizer<UpdateTraderRequestValidator> localizer)
    {
        RuleFor(t => t.FirstName).NotEmpty().MinimumLength(3).MaximumLength(20);
        RuleFor(t => t.LastName).NotEmpty().MinimumLength(3).MaximumLength(20);

        RuleFor(t => t.Email)
            .NotEmpty()
            .EmailAddress()
                .WithMessage(localizer["email.invalid"])
            .MustAsync(async (trader, email, ct) =>
                    await repository.GetBySpecAsync(new TraderByEmailSpec(email, currentUser.GetUserId()), ct)
                        is not Trader existingTrader || existingTrader.Id == trader.Id)
                .WithMessage((_, email) => string.Format(localizer["trader.alreadyexists"], email));
    }
}

public class UpdateTraderRequestHandler : IRequestHandler<UpdateTraderRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Trader> _repository;
    private readonly IStringLocalizer<UpdateTraderRequestHandler> _localizer;

    public UpdateTraderRequestHandler(IRepositoryWithEvents<Trader> repository, IStringLocalizer<UpdateTraderRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<Guid> Handle(UpdateTraderRequest request, CancellationToken cancellationToken)
    {
        var trader = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = trader ?? throw new NotFoundException(string.Format(_localizer["trader.notfound"], request.Id));

        trader.Update(request.FirstName, request.LastName, request.Email);

        await _repository.UpdateAsync(trader, cancellationToken);

        return request.Id;
    }
}