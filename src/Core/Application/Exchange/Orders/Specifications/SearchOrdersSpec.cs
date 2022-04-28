using FSH.WebApi.Application.Exchange.Orders.DTOs;

namespace FSH.WebApi.Application.Exchange.Orders.Specifications;

public class SearchOrdersSpec : EntitiesByPaginationFilterSpec<Order, OrderDto>
{
    public SearchOrdersSpec(SearchOrdersRequest request, Guid userId)
        : base(request) => Query
            .Where(o => o.CreatedBy == userId)
            .Where(o => o.Status == request.Status, request.Status is not null)
            .Where(o => o.TraderId == request.TraderId, request.TraderId is not null)
            .OrderByDescending(o => o.Id, !request.HasOrderBy());
}