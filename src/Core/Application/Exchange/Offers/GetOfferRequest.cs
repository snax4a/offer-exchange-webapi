using FSH.WebApi.Application.Exchange.Offers.Specifications;

namespace FSH.WebApi.Application.Exchange.Offers;

public class GetOfferRequest : IRequest<OfferDetailsDto>
{
    public Guid Id { get; set; }

    public GetOfferRequest(Guid id) => Id = id;
}

public class GetOfferRequestHandler : IRequestHandler<GetOfferRequest, OfferDetailsDto>
{
    private readonly ICurrentUser _currentUser;
    private readonly IRepository<Offer> _repository;
    private readonly IStringLocalizer<GetOfferRequestHandler> _localizer;

    public GetOfferRequestHandler(ICurrentUser currentUser, IRepository<Offer> repository, IStringLocalizer<GetOfferRequestHandler> localizer) => (_currentUser, _repository, _localizer) = (currentUser, repository, localizer);

    public async Task<OfferDetailsDto> Handle(GetOfferRequest request, CancellationToken cancellationToken)
    {
        ISpecification<Offer, OfferDetailsDto> spec = new OfferDetailsSpec(request.Id, _currentUser.GetUserId());
        var offer = await _repository.GetBySpecAsync(spec, cancellationToken);

        if (offer is not null) return offer;

        throw new NotFoundException(string.Format(_localizer["offer.notfound"], request.Id));
    }
}