using FSH.WebApi.Application.Exchange.Traders.Specifications;

namespace FSH.WebApi.Application.Exchange.Traders;

public class CreateTraderRequest : IRequest<Guid>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public IList<Guid> GroupIds { get; set; } = default!;
}

public class CreateTraderRequestValidator : CustomValidator<CreateTraderRequest>
{
    public CreateTraderRequestValidator(
        ICurrentUser currentUser,
        IReadRepository<Trader> traderRepo,
        IReadRepository<Group> groupRepo,
        IStringLocalizer<CreateTraderRequestValidator> localizer)
    {
        RuleFor(t => t.FirstName).NotEmpty().MinimumLength(3).MaximumLength(20);
        RuleFor(t => t.LastName).NotEmpty().MinimumLength(3).MaximumLength(20);

        RuleFor(t => t.Email)
            .NotEmpty()
            .EmailAddress()
                .WithMessage(localizer["email.invalid"])
            .MustAsync(async (email, ct) => await traderRepo.GetBySpecAsync(new TraderByEmailSpec(email, currentUser.GetUserId()), ct) is null)
                .WithMessage((_, email) => string.Format(localizer["trader.alreadyexists"], email));

        RuleForEach(t => t.GroupIds)
            .MustAsync(async (groupId, ct) => await groupRepo.GetByIdAsync(groupId, ct) is not null)
                .WithMessage((_, groupId) => string.Format(localizer["group.notfound"], groupId));
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

        if (request.GroupIds.Count > 0)
        {
            foreach (Guid groupId in request.GroupIds)
                trader.AddGroup(groupId);
        }

        await _repository.AddAsync(trader, cancellationToken);

        return trader.Id;
    }
}