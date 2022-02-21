namespace FSH.WebApi.Application.Exchange.Traders;

public class UpdateTraderRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public IList<Guid> GroupIds { get; set; } = default!;
}

public class UpdateTraderRequestValidator : CustomValidator<UpdateTraderRequest>
{
    public UpdateTraderRequestValidator(
        ICurrentUser currentUser,
        IRepository<Trader> traderRepo,
        IRepository<Group> groupRepo,
        IStringLocalizer<UpdateTraderRequestValidator> localizer)
    {
        RuleFor(t => t.FirstName).NotEmpty().MinimumLength(3).MaximumLength(20);
        RuleFor(t => t.LastName).NotEmpty().MinimumLength(3).MaximumLength(20);

        RuleFor(t => t.Email)
            .NotEmpty()
            .EmailAddress()
                .WithMessage(localizer["email.invalid"])
            .MustAsync(async (trader, email, ct) =>
                    await traderRepo.GetBySpecAsync(new TraderByEmailSpec(email, currentUser.GetUserId()), ct)
                        is not Trader existingTrader || existingTrader.Email == trader.Email)
                .WithMessage((_, email) => string.Format(localizer["trader.alreadyexists"], email));

        RuleForEach(t => t.GroupIds)
            .MustAsync(async (groupId, ct) => await groupRepo.GetByIdAsync(groupId, ct) is not null)
                .WithMessage((_, groupId) => string.Format(localizer["group.notfound"], groupId));
    }
}

public class UpdateTraderRequestHandler : IRequestHandler<UpdateTraderRequest, Guid>
{
    private readonly ICurrentUser _currentUser;

    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Trader> _repository;
    private readonly IStringLocalizer<UpdateTraderRequestHandler> _localizer;

    public UpdateTraderRequestHandler(ICurrentUser currentUser, IRepositoryWithEvents<Trader> repository, IStringLocalizer<UpdateTraderRequestHandler> localizer) =>
        (_currentUser, _repository, _localizer) = (currentUser, repository, localizer);

    public async Task<Guid> Handle(UpdateTraderRequest request, CancellationToken cancellationToken)
    {
        var trader = await _repository.GetBySpecAsync(new TraderDetailsSpec(request.Id, _currentUser.GetUserId()), cancellationToken);

        _ = trader ?? throw new NotFoundException(string.Format(_localizer["trader.notfound"], request.Id));

        trader.ClearGroups();

        foreach (Guid groupId in request.GroupIds)
        {
            trader.AddGroup(groupId);
        }

        trader.Update(request.FirstName, request.LastName, request.Email);

        await _repository.UpdateAsync(trader, cancellationToken);

        return request.Id;
    }
}