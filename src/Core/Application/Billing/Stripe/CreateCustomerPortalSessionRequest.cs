using FSH.WebApi.Application.Billing.Stripe;
using FSH.WebApi.Application.Exchange.Billing.Customers.Specifications;
using FSH.WebApi.Application.Exchange.Billing.Stripe.DTOs;
using FSH.WebApi.Domain.Billing;

namespace FSH.WebApi.Application.Exchange.Billing.Stripe;

public class CreateCustomerPortalSessionRequest : IRequest<StripeCustomerPortalSessionDto>
{
}

public class CreateCustomerPortalSessionRequestHandler : IRequestHandler<CreateCustomerPortalSessionRequest, StripeCustomerPortalSessionDto>
{
    private readonly ICurrentUser _currentUser;
    private readonly IStripeService _stripeService;
    private readonly IReadRepository<Customer> _customerRepository;

    public CreateCustomerPortalSessionRequestHandler(
        ICurrentUser currentUser,
        IStripeService stripeService,
        IReadRepository<Customer> customerRepository)
    {
        _currentUser = currentUser;
        _stripeService = stripeService;
        _customerRepository = customerRepository;
    }

    public async Task<StripeCustomerPortalSessionDto> Handle(CreateCustomerPortalSessionRequest request, CancellationToken ct)
    {
        Guid userId = _currentUser.GetUserId();
        var spec = new CustomerByUserIdSpec(userId);
        var customer = await _customerRepository.GetBySpecAsync(spec, ct);

        _ = customer ?? throw new NotFoundException($"Customer for userId: {userId} not found.");

        return await _stripeService.CreateCustomerPortalSession(customer.StripeCustomerId, ct);
    }
}