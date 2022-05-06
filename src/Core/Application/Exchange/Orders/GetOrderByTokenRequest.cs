using FSH.WebApi.Application.Exchange.Orders.DTOs;
using FSH.WebApi.Application.Exchange.Orders.Specifications;
using FSH.WebApi.Application.Identity.Users;
using Mapster;

namespace FSH.WebApi.Application.Exchange.Orders;

public class GetOrderByTokenRequest : IRequest<OrderByTokenDto>
{
    public string OrderToken { get; set; }

    public GetOrderByTokenRequest(string orderToken) => OrderToken = orderToken;
}

public class GetOrderByTokenRequestHandler : IRequestHandler<GetOrderByTokenRequest, OrderByTokenDto>
{
    private readonly IReadRepository<Order> _orderRepo;
    private readonly IUserService _userService;
    private readonly IOrderTokenService _tokenService;
    private readonly IStringLocalizer<GetOrderByTokenRequestHandler> _localizer;

    public GetOrderByTokenRequestHandler(
        IReadRepository<Order> orderRepo,
        IUserService userService,
        IOrderTokenService tokenService,
        IStringLocalizer<GetOrderByTokenRequestHandler> localizer)
    {
        _orderRepo = orderRepo;
        _userService = userService;
        _tokenService = tokenService;
        _localizer = localizer;
    }

    public async Task<OrderByTokenDto> Handle(GetOrderByTokenRequest request, CancellationToken ct)
    {
        // Decode orderId and traderId from offer token
        (Guid orderId, Guid traderId) = _tokenService.DecodeToken(request.OrderToken);

        var spec = new OrderDetailsByIdAndTraderSpec(orderId, traderId);
        var order = await _orderRepo.GetBySpecAsync(spec, ct);

        if (order is null || order.TraderId != traderId)
            throw new NotFoundException(string.Format(_localizer["order.notfound"], orderId));

        var user = await _userService.GetAsync(order.CreatedBy.ToString(), ct);

        return new OrderByTokenDto()
        {
            Order = order.Adapt<Order, OrderDetailsDto>(),
            Creator = user.Adapt<UserDto>()
        };
    }
}