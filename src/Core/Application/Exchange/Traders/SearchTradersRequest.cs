namespace FSH.WebApi.Application.Exchange.Traders;

public class SearchTradersRequest : PaginationFilter, IRequest<PaginationResponse<TraderDto>>
{
}

public class SearchTradersSpec : EntitiesByPaginationFilterSpec<Trader, TraderDto>
{
    public SearchTradersSpec(SearchTradersRequest request, Guid userId)
        : base(request) => Query
            .Where(t => t.CreatedBy == userId)
            .Include(t => t.TraderGroups)
                .ThenInclude(tg => tg.Group)
            .OrderBy(t => new { t.FirstName, t.LastName }, !request.HasOrderBy());
}

public class SearchTradersRequestHandler : IRequestHandler<SearchTradersRequest, PaginationResponse<TraderDto>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IReadRepository<Trader> _repository;

    public SearchTradersRequestHandler(ICurrentUser currentUser, IReadRepository<Trader> repository) =>
        (_currentUser, _repository) = (currentUser, repository);

    public async Task<PaginationResponse<TraderDto>> Handle(SearchTradersRequest request, CancellationToken cancellationToken)
    {
        var spec = new SearchTradersSpec(request, _currentUser.GetUserId());

        var list = await _repository.ListAsync(spec, cancellationToken);
        int count = await _repository.CountAsync(spec, cancellationToken);

        return new PaginationResponse<TraderDto>(list, count, request.PageNumber, request.PageSize);
    }
}