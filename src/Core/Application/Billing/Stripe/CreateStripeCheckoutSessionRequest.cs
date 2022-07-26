using FSH.WebApi.Application.Billing.Stripe;
using FSH.WebApi.Application.Exchange.Billing.Customers.Specifications;
using FSH.WebApi.Application.Exchange.Billing.Stripe.DTOs;
using FSH.WebApi.Application.Exchange.Billing.Stripe.Specifications;
using FSH.WebApi.Domain.Billing;

namespace FSH.WebApi.Application.Exchange.Billing.Stripe;

public class CreateStripeCheckoutSessionRequest : IRequest<StripeCheckoutSessionDto>
{
    public string PriceId { get; set; } = default!;
}

public class CreateStripeCheckoutSessionRequestValidator : CustomValidator<CreateStripeCheckoutSessionRequest>
{
    public CreateStripeCheckoutSessionRequestValidator()
    {
        CascadeMode = CascadeMode.Stop;
        RuleFor(r => r.PriceId)
            .NotEmpty()
            .Must(priceId => priceId.StartsWith("price_"))
            .WithMessage("Provided PriceId is invalid.");
    }
}

public class CreateStripeCheckoutSessionRequestHandler : IRequestHandler<CreateStripeCheckoutSessionRequest, StripeCheckoutSessionDto>
{
    private readonly ICurrentUser _currentUser;
    private readonly IStripeService _stripeService;
    private readonly IReadRepository<Customer> _customerRepository;
    private readonly IReadRepository<StripeSubscription> _subscriptionRepository;

    public CreateStripeCheckoutSessionRequestHandler(
        ICurrentUser currentUser,
        IStripeService stripeService,
        IReadRepository<Customer> customerRepository,
        IReadRepository<StripeSubscription> subscriptionRepository)
    {
        _currentUser = currentUser;
        _stripeService = stripeService;
        _customerRepository = customerRepository;
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<StripeCheckoutSessionDto> Handle(CreateStripeCheckoutSessionRequest request, CancellationToken ct)
    {
        Guid userId = _currentUser.GetUserId();
        var spec = new CustomerByUserIdSpec(userId);
        var customer = await _customerRepository.GetBySpecAsync(spec, ct);

        _ = customer ?? throw new NotFoundException($"Customer for userId: {userId} not found.");

        // Check if customer has active subscription.
        var subscriptionsSpec = new SubscriptionsByCustomerIdAndStatusSpec(customer.StripeCustomerId, "active");
        int activeSubscriptionCount = await _subscriptionRepository.CountAsync(subscriptionsSpec, ct);

        if (activeSubscriptionCount > 0)
            throw new ConflictException("Customer already has active subscription.");

        return await _stripeService.CreateCheckoutSession(customer.StripeCustomerId, request.PriceId, ct);
    }
}