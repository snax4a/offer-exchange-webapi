using FSH.WebApi.Application.Exchange.Offers.Specifications;

namespace FSH.WebApi.Application.Exchange.Offers;

public class SearchOffersRequest : PaginationFilter, IRequest<PaginationResponse<OfferDto>>
{
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
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);
    }
}