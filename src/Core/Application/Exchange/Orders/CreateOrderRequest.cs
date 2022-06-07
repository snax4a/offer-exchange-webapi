using FSH.WebApi.Application.Exchange.Offers.Specifications;

namespace FSH.WebApi.Application.Exchange.Orders;

public class CreateOrderRequest : IRequest<List<Guid>>
{
    public IList<Guid> ProductIds { get; set; } = default!;
}

public class CreateOrderRequestValidator : CustomValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator(IStringLocalizer<CreateOrderRequestValidator> localizer)
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(o => o.ProductIds)
            .Must(ids => ids.Count > 0)
            .WithMessage(localizer["order.noproducts"]);
    }
}

public class CreateOrderRequestHandler : IRequestHandler<CreateOrderRequest, List<Guid>>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Order> _repository;
    private readonly IReadRepository<OfferProduct> _productRepo;
    private readonly ICurrentUser _currentUser;
    private readonly IJobService _jobService;

    public CreateOrderRequestHandler(
        IRepositoryWithEvents<Order> repository,
        IReadRepository<OfferProduct> productRepo,
        ICurrentUser currentUser,
        IJobService jobService)
    {
        _repository = repository;
        _productRepo = productRepo;
        _currentUser = currentUser;
        _jobService = jobService;
    }

    public async Task<List<Guid>> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        Dictionary<Guid, List<OfferProduct>> traderProducts = new();

        // Prepare dictionary of traders containing their offer products
        foreach (Guid productId in request.ProductIds)
        {
            var spec = new OfferProductForOrderSpec(productId, _currentUser.GetUserId());
            var product = await _productRepo.GetBySpecAsync(spec, cancellationToken);

            if (product is null)
                throw new NotFoundException(string.Format("Offer product {0} not found.", productId));

            Guid traderId = product.Offer.TraderId;

            if (traderProducts.ContainsKey(traderId))
                traderProducts[traderId].Add(product);
            else
                traderProducts.Add(traderId, new List<OfferProduct>() { product });
        }

        List<Guid> orderIds = new();

        // TODO: When AddRangeAsync will be available in RepositoryBase then use it
        foreach (var kvp in traderProducts)
        {
            var traderId = kvp.Key;
            var orderProducts = kvp.Value;
            var order = new Order(traderId, orderProducts);
            await _repository.AddAsync(order, cancellationToken);
            _jobService.Enqueue<IOrderSenderJob>(x => x.SendAsync(order.Id, CancellationToken.None));
            orderIds.Add(order.Id);
        }

        return orderIds;
    }
}