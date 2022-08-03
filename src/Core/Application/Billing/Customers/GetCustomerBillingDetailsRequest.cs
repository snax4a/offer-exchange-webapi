using FSH.WebApi.Application.Exchange.Billing.Customers.DTOs;
using FSH.WebApi.Application.Exchange.Billing.Customers.Specifications;
using FSH.WebApi.Domain.Billing;
using Mapster;

namespace FSH.WebApi.Application.Exchange.Billing.Customers;

public class GetCustomerBillingDetailsRequest : IRequest<CustomerBillingDetailsDto>
{
}

public class GetCustomerBillingDetailsRequestHandler : IRequestHandler<GetCustomerBillingDetailsRequest, CustomerBillingDetailsDto>
{
    private readonly ICurrentUser _currentUser;
    private readonly IReadRepository<Customer> _customerRepository;

    public GetCustomerBillingDetailsRequestHandler(ICurrentUser currentUser, IReadRepository<Customer> customerRepository)
    {
        _currentUser = currentUser;
        _customerRepository = customerRepository;
    }

    public async Task<CustomerBillingDetailsDto> Handle(GetCustomerBillingDetailsRequest request, CancellationToken ct)
    {
        Guid userId = _currentUser.GetUserId();
        var spec = new CustomerWithSubscriptionByUserIdSpec(userId);
        var customer = await _customerRepository.GetBySpecAsync(spec, ct);

        _ = customer ?? throw new NotFoundException($"Customer for userId: {userId} not found.");

        return customer.Adapt<CustomerBillingDetailsDto>();
    }
}