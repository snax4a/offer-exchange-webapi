using FSH.WebApi.Application.Exchange.Offers;

namespace FSH.WebApi.Application.Exchange.Traders;

public class DeleteTraderRequest : IRequest<Guid>
{
    public Guid Id { get; set; }

    public DeleteTraderRequest(Guid id) => Id = id;
}

public class DeleteTraderRequestHandler : IRequestHandler<DeleteTraderRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Trader> _traderRepo;
    private readonly IReadRepository<Offer> _offerRepo;
    private readonly IStringLocalizer<DeleteTraderRequestHandler> _localizer;

    public DeleteTraderRequestHandler(
        IRepositoryWithEvents<Trader> traderRepo,
        IReadRepository<Offer> offerRepo,
        IStringLocalizer<DeleteTraderRequestHandler> localizer)
    {
        (_traderRepo, _offerRepo, _localizer) = (traderRepo, offerRepo, localizer);
    }

    public async Task<Guid> Handle(DeleteTraderRequest request, CancellationToken cancellationToken)
    {
        if (await _offerRepo.AnyAsync(new OffersByTraderSpec(request.Id), cancellationToken))
        {
            throw new ConflictException(_localizer["trader.cannotbedeleted"]);
        }

        // TODO: When orders are implemented add check if any order for this trader exist

        var trader = await _traderRepo.GetByIdAsync(request.Id, cancellationToken);

        _ = trader ?? throw new NotFoundException(string.Format(_localizer["trader.notfound"], request.Id));

        await _traderRepo.DeleteAsync(trader, cancellationToken);

        return request.Id;
    }
}