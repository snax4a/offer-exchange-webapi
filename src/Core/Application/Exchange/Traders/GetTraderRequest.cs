namespace FSH.WebApi.Application.Exchange.Traders;

public class GetTraderRequest : IRequest<TraderDto>
{
    public Guid Id { get; set; }

    public GetTraderRequest(Guid id) => Id = id;
}

public class GetTraderRequestHandler : IRequestHandler<GetTraderRequest, TraderDto>
{
    private readonly ICurrentUser _currentUser;
    private readonly IRepository<Trader> _repository;
    private readonly IStringLocalizer<GetTraderRequestHandler> _localizer;

    public GetTraderRequestHandler(ICurrentUser currentUser, IRepository<Trader> repository, IStringLocalizer<GetTraderRequestHandler> localizer) => (_currentUser, _repository, _localizer) = (currentUser, repository, localizer);

    public async Task<TraderDto> Handle(GetTraderRequest request, CancellationToken cancellationToken)
    {
        ISpecification<Trader, TraderDto> spec = new TraderDetailsSpec(request.Id, _currentUser.GetUserId());
        var trader = await _repository.GetBySpecAsync(spec, cancellationToken);

        if (trader is not null) return trader;

        throw new NotFoundException(string.Format(_localizer["trader.notfound"], request.Id));
    }
}