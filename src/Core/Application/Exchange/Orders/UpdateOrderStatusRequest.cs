using FSH.WebApi.Application.Exchange.Orders.Specifications;

namespace FSH.WebApi.Application.Exchange.Orders;

public class UpdateOrderStatusRequest : IRequest<Guid>
{
    public OrderStatus Status { get; set; }
    public string OrderToken { get; set; } = default!;
}

public class UpdateOrderStatusRequestValidator : CustomValidator<UpdateOrderStatusRequest>
{
    public UpdateOrderStatusRequestValidator()
    {
        CascadeMode = CascadeMode.Stop;
        RuleFor(p => p.Status).NotNull();
    }
}

public class UpdateOrderStatusRequestHandler : IRequestHandler<UpdateOrderStatusRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Order> _repository;
    private readonly IStringLocalizer<UpdateOrderStatusRequestHandler> _localizer;
    private readonly IOrderTokenService _tokenService;

    public UpdateOrderStatusRequestHandler(
        IRepositoryWithEvents<Order> repository,
        IStringLocalizer<UpdateOrderStatusRequestHandler> localizer,
        IOrderTokenService tokenService)
    {
        _repository = repository;
        _localizer = localizer;
        _tokenService = tokenService;
    }

    public async Task<Guid> Handle(UpdateOrderStatusRequest request, CancellationToken cancellationToken)
    {
        (Guid orderId, Guid traderId) = _tokenService.DecodeToken(request.OrderToken);
        var spec = new OrderByIdAndTraderSpec(orderId, traderId);
        var order = await _repository.GetBySpecAsync(spec, cancellationToken);

        _ = order ?? throw new NotFoundException(string.Format(_localizer["order.notfound"], orderId));

        try
        {
            order.UpdateStatus(request.Status);
            await _repository.UpdateAsync(order, cancellationToken);
        }
        catch (Exception)
        {
            throw new ConflictException(_localizer["order.status-cannot-be-changed"]);
        }

        return orderId;
    }
}