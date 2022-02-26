using FSH.WebApi.Application.Exchange.Traders.Specifications;

namespace FSH.WebApi.Application.Exchange.Traders;

public class SearchTradersRequest : PaginationFilter, IRequest<PaginationResponse<TraderDetailsDto>>
{
}

public class SearchTradersRequestHandler : IRequestHandler<SearchTradersRequest, PaginationResponse<TraderDetailsDto>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IReadRepository<Trader> _repository;

    public SearchTradersRequestHandler(ICurrentUser currentUser, IReadRepository<Trader> repository)
    {
        (_currentUser, _repository) = (currentUser, repository);
    }

    public async Task<PaginationResponse<TraderDetailsDto>> Handle(SearchTradersRequest request, CancellationToken cancellationToken)
    {
        var spec = new SearchTradersSpec(request, _currentUser.GetUserId());
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);
    }
}