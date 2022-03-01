namespace FSH.WebApi.Application.Exchange.Orders.Specifications;

public class SearchOrdersSpec : EntitiesByPaginationFilterSpec<Order, OrderDto>
{
    public SearchOrdersSpec(SearchOrdersRequest request, Guid userId)
        : base(request) => Query
            .Where(o => o.CreatedBy == userId)
            .OrderByDescending(o => o.Id, !request.HasOrderBy());
}