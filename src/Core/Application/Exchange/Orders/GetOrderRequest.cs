using FSH.WebApi.Application.Exchange.Orders.DTOs;
using FSH.WebApi.Application.Exchange.Orders.Specifications;

namespace FSH.WebApi.Application.Exchange.Orders;

public class GetOrderRequest : IRequest<OrderDetailsDto>
{
    public Guid Id { get; set; }

    public GetOrderRequest(Guid id) => Id = id;
}

public class GetOrderRequestHandler : IRequestHandler<GetOrderRequest, OrderDetailsDto>
{
    private readonly ICurrentUser _currentUser;
    private readonly IRepository<Order> _repository;
    private readonly IStringLocalizer<GetOrderRequestHandler> _localizer;

    public GetOrderRequestHandler(
        ICurrentUser currentUser,
        IRepository<Order> repository,
        IStringLocalizer<GetOrderRequestHandler> localizer)
    {
        (_currentUser, _repository, _localizer) = (currentUser, repository, localizer);
    }

    public async Task<OrderDetailsDto> Handle(GetOrderRequest request, CancellationToken cancellationToken)
    {
        ISpecification<Order, OrderDetailsDto> spec = new OrderDetailsSpec(request.Id, _currentUser.GetUserId());
        var order = await _repository.GetBySpecAsync(spec, cancellationToken);

        if (order is not null) return order;

        throw new NotFoundException(string.Format(_localizer["order.notfound"], request.Id));
    }
}