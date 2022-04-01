using FSH.WebApi.Application.Exchange.Orders.DTOs;
using FSH.WebApi.Application.Exchange.Orders.Specifications;

namespace FSH.WebApi.Application.Exchange.Orders;

public class SearchOrdersRequest : PaginationFilter, IRequest<PaginationResponse<OrderDto>>
{
}

public class SearchOrdersRequestHandler : IRequestHandler<SearchOrdersRequest, PaginationResponse<OrderDto>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IReadRepository<Order> _repository;

    public SearchOrdersRequestHandler(ICurrentUser currentUser, IReadRepository<Order> repository)
    {
        (_currentUser, _repository) = (currentUser, repository);
    }

    public async Task<PaginationResponse<OrderDto>> Handle(SearchOrdersRequest request, CancellationToken cancellationToken)
    {
        var spec = new SearchOrdersSpec(request, _currentUser.GetUserId());
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);
    }
}