using FSH.WebApi.Application.Exchange.Groups.Specifications;

namespace FSH.WebApi.Application.Exchange.Groups;

public class UpdateGroupRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public ColorName Color { get; set; }
}

public class UpdateGroupRequestValidator : CustomValidator<UpdateGroupRequest>
{
    public UpdateGroupRequestValidator(IRepository<Group> repository, ICurrentUser currentUser, IStringLocalizer<UpdateGroupRequestValidator> localizer)
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(p => p.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(20)
            .MustAsync(async (group, name, ct) =>
                    await repository.GetBySpecAsync(new GroupByNameAndOwnerSpec(name, currentUser.GetUserId()), ct)
                        is not Group existingGroup || existingGroup.Id == group.Id)
                .WithMessage((_, name) => string.Format(localizer["group.alreadyexists"], name));

        RuleFor(g => g.Color).NotEmpty();
    }
}

public class UpdateGroupRequestHandler : IRequestHandler<UpdateGroupRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Group> _repository;
    private readonly IStringLocalizer<UpdateGroupRequestHandler> _localizer;

    public UpdateGroupRequestHandler(IRepositoryWithEvents<Group> repository, IStringLocalizer<UpdateGroupRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<Guid> Handle(UpdateGroupRequest request, CancellationToken cancellationToken)
    {
        var group = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = group ?? throw new NotFoundException(string.Format(_localizer["group.notfound"], request.Id));

        group.Update(request.Name, request.Color);

        await _repository.UpdateAsync(group, cancellationToken);

        return request.Id;
    }
}