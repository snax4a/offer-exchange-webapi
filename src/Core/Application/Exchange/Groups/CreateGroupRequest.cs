namespace FSH.WebApi.Application.Exchange.Groups;

public class CreateGroupRequest : IRequest<Guid>
{
    public string Name { get; set; } = default!;
    public ColorName Color { get; set; }
}

public class CreateGroupRequestValidator : CustomValidator<CreateGroupRequest>
{
    public CreateGroupRequestValidator(IReadRepository<Group> repository, ICurrentUser currentUser, IStringLocalizer<CreateGroupRequestValidator> localizer)
    {
        RuleFor(g => g.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(20)
            .MustAsync(async (name, ct) => await repository.GetBySpecAsync(new GroupByNameAndOwnerSpec(name, currentUser.GetUserId()), ct) is null)
                .WithMessage((_, name) => string.Format(localizer["group.alreadyexists"], name));

        RuleFor(g => g.Color).NotEmpty();
    }
}

public class CreateGroupRequestHandler : IRequestHandler<CreateGroupRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Group> _repository;

    public CreateGroupRequestHandler(IRepositoryWithEvents<Group> repository) => _repository = repository;

    public async Task<Guid> Handle(CreateGroupRequest request, CancellationToken cancellationToken)
    {
        var group = new Group(request.Name, request.Color);

        await _repository.AddAsync(group, cancellationToken);

        return group.Id;
    }
}