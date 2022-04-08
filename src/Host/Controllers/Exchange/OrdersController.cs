using FSH.WebApi.Application.Exchange.Orders;
using FSH.WebApi.Application.Exchange.Orders.DTOs;

namespace FSH.WebApi.Host.Controllers.Exchange;

public class OrdersController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(ResourceAction.Search, Resource.Orders)]
    [OpenApiOperation("Search orders using available filters.", "")]
    public Task<PaginationResponse<OrderDto>> SearchAsync(SearchOrdersRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(ResourceAction.View, Resource.Orders)]
    [OpenApiOperation("Get order details.", "")]
    public Task<OrderDetailsDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetOrderRequest(id));
    }

    [HttpPost]
    [MustHavePermission(ResourceAction.Create, Resource.Orders)]
    [OpenApiOperation("Create a new order.", "")]
    public Task<List<Guid>> CreateAsync(CreateOrderRequest request)
    {
        return Mediator.Send(request);
    }
}