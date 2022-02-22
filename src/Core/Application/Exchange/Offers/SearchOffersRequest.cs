namespace FSH.WebApi.Application.Exchange.Offers;

public class SearchOffersRequest : PaginationFilter, IRequest<PaginationResponse<OfferDto>>
{
}

public class SearchOffersSpec : EntitiesByPaginationFilterSpec<Offer, OfferDto>
{
    public SearchOffersSpec(SearchOffersRequest request, Guid userId)
        : base(request) => Query
            .Where(o => o.UserId == userId)
            .OrderBy(o => o.Id, !request.HasOrderBy());
}

public class SearchOffersRequestHandler : IRequestHandler<SearchOffersRequest, PaginationResponse<OfferDto>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IReadRepository<Offer> _repository;

    public SearchOffersRequestHandler(ICurrentUser currentUser, IReadRepository<Offer> repository) =>
        (_currentUser, _repository) = (currentUser, repository);

    public async Task<PaginationResponse<OfferDto>> Handle(SearchOffersRequest request, CancellationToken cancellationToken)
    {
        var spec = new SearchOffersSpec(request, _currentUser.GetUserId());

        var list = await _repository.ListAsync(spec, cancellationToken);
        int count = await _repository.CountAsync(spec, cancellationToken);

        return new PaginationResponse<OfferDto>(list, count, request.PageNumber, request.PageSize);
    }
}