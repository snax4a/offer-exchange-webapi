using FSH.WebApi.Application.Exchange.Traders;

namespace FSH.WebApi.Application.Exchange.Groups;

public class DeleteGroupRequest : IRequest<Guid>
{
    public Guid Id { get; set; }

    public DeleteGroupRequest(Guid id) => Id = id;
}

public class DeleteGroupRequestHandler : IRequestHandler<DeleteGroupRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Group> _groupRepo;
    private readonly IReadRepository<Trader> _traderRepo;
    private readonly IStringLocalizer<DeleteGroupRequestHandler> _localizer;

    public DeleteGroupRequestHandler(IRepositoryWithEvents<Group> groupRepo, IReadRepository<Trader> traderRepo, IStringLocalizer<DeleteGroupRequestHandler> localizer) =>
        (_groupRepo, _traderRepo, _localizer) = (groupRepo, traderRepo, localizer);

    public async Task<Guid> Handle(DeleteGroupRequest request, CancellationToken cancellationToken)
    {
        if (await _traderRepo.AnyAsync(new TradersByGroupSpec(request.Id), cancellationToken))
        {
            throw new ConflictException(_localizer["group.cannotbedeleted"]);
        }

        var group = await _groupRepo.GetByIdAsync(request.Id, cancellationToken);

        _ = group ?? throw new NotFoundException(string.Format(_localizer["group.notfound"], request.Id));

        await _groupRepo.DeleteAsync(group, cancellationToken);

        return request.Id;
    }
}