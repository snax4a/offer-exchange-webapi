using FSH.WebApi.Application.Exchange.Traders.Specifications;

namespace FSH.WebApi.Application.Exchange.Traders;

public class GetTradersByGroupsRequest : IRequest<List<TraderDto>>
{
    public IList<Guid> GroupIds { get; set; } = default!;
}

public class GetTradersByGroupsRequestHandler : IRequestHandler<GetTradersByGroupsRequest, List<TraderDto>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IRepository<Trader> _repository;

    public GetTradersByGroupsRequestHandler(
        ICurrentUser currentUser,
        IRepository<Trader> repository)
    {
        (_currentUser, _repository) = (currentUser, repository);
    }

    public async Task<List<TraderDto>> Handle(GetTradersByGroupsRequest request, CancellationToken cancellationToken)
    {
        var spec = new TradersByGroupsSpec(request.GroupIds, _currentUser.GetUserId());
        return await _repository.ListAsync(spec, cancellationToken);
    }
}