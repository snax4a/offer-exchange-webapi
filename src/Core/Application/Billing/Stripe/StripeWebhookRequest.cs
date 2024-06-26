using FSH.WebApi.Application.Billing.Stripe;
using FSH.WebApi.Application.Exchange.Billing.Customers.Specifications;
using FSH.WebApi.Domain.Billing;
using Stripe;

namespace FSH.WebApi.Application.Exchange.Billing.Stripe;

public class StripeWebhookRequest : IRequest<bool>
{
    public string Payload { get; set; } = default!;
    public string Signature { get; set; } = default!;
    public StripeWebhookRequest(string payload, string signature) => (Payload, Signature) = (payload, signature);
}

public class StripeWebhookRequestValidator : CustomValidator<StripeWebhookRequest>
{
    public StripeWebhookRequestValidator()
    {
        CascadeMode = CascadeMode.Stop;
        RuleFor(r => r.Payload).NotEmpty();
        RuleFor(r => r.Signature).NotEmpty();
    }
}

public class StripeWebhookRequestHandler : IRequestHandler<StripeWebhookRequest, bool>
{
    private readonly IStripeService _stripeService;
    private readonly IRepositoryWithEvents<Domain.Billing.Customer> _customerRepository;
    private readonly IRepositoryWithEvents<StripeEvent> _eventRepository;
    private readonly ILogger<StripeWebhookRequestHandler> _logger;

    public StripeWebhookRequestHandler(
        IStripeService stripeService,
        IRepositoryWithEvents<Domain.Billing.Customer> customerRepository,
        IRepositoryWithEvents<StripeEvent> eventRepository,
        ILogger<StripeWebhookRequestHandler> logger)
    {
        _stripeService = stripeService;
        _customerRepository = customerRepository;
        _eventRepository = eventRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(StripeWebhookRequest request, CancellationToken ct)
    {
        try
        {
            Event stripeEvent = _stripeService.ConstructEvent(request.Payload, request.Signature);

            _logger.LogInformation($"Processing stripe webhook request with type: {stripeEvent.Type} found for: {stripeEvent.Id}");

            // To avoid issues with duplicate events, we will only process events that have not been previously processed
            if (await WasEventAlreadyProcessed(stripeEvent.Id, ct))
            {
                _logger.LogInformation($"The event with id: {stripeEvent.Id} has already been processed, skipping...");
                return true;
            }

            switch (stripeEvent.Type)
            {
                case "customer.subscription.updated":
                case "customer.subscription.deleted":
                    var subscription = (Subscription)stripeEvent.Data.Object;
                    await HandleSubscriptionStatusChanged(subscription, ct);
                    break;
                case "invoice.paid":
                    var invoice = (Invoice)stripeEvent.Data.Object;
                    await HandleInvoicePaid(invoice, ct);
                    break;
                case "invoice.payment_failed":
                    // The payment failed or the customer does not have a valid payment method.
                    // The subscription becomes past_due. We should notify customer and send them to the
                    // customer portal to update their payment information.
                    // TODO: implement notification
                    break;
                case "product.created":
                case "product.updated":
                    var product = (Product)stripeEvent.Data.Object;
                    await HandleProductChanged(product, ct);
                    break;
                case "price.created":
                case "price.updated":
                    var price = (Price)stripeEvent.Data.Object;
                    await HandlePriceChanged(price, ct);
                    break;
                default:
                    _logger.LogWarning($"Unhandled stripe webhook request type: {stripeEvent.Type}");
                    return false;
            }

            await StoreProcessedEvent(stripeEvent, CancellationToken.None);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Stripe webhook request failed.");
            throw new InternalServerException("Stripe webhook request failed.");
        }

        return true;
    }

    private async Task HandleSubscriptionStatusChanged(Subscription subscription, CancellationToken ct)
    {
        // Update the subscription data in the database.
        string newStatus = subscription.Status;
        await _stripeService.UpsertSubscription(subscription, ct);
        _logger.LogInformation($"Subscription: {subscription.Id} data was updated. Status: {newStatus}");

        if (newStatus == "canceled" || newStatus == "unpaid")
        {
            string customerId = subscription.CustomerId;
            var spec = new CustomerByStripeCustomerIdSpec(customerId);
            var customer = await _customerRepository.GetBySpecAsync(spec, ct);

            _ = customer ?? throw new InternalServerException(
                $"Customer: {customerId} not found. Was unable to reset his billing plan.");

            customer.SetBillingPlan(BillingPlan.Free);
            customer.SetCurrentSubscription(null);
            await _customerRepository.UpdateAsync(customer, ct);

            _logger.LogInformation($"Customer: {customerId} billing plan was set to: {customer.BillingPlan}");
        }
    }

    private async Task HandleInvoicePaid(Invoice invoice, CancellationToken ct)
    {
        // TODO: consider checking BillingReason property to see why invoice has been created.

        if (string.IsNullOrEmpty(invoice.SubscriptionId))
        {
            _logger.LogWarning($"Invoice: {invoice.Id} is not associated with a subscription.");
            return;
        }

        var stripeSubscription = await _stripeService.RetrieveSubscription(invoice.SubscriptionId, ct);
        await _stripeService.UpsertSubscription(stripeSubscription, ct);

        if (stripeSubscription.Status != "active")
        {
            _logger.LogWarning($"Incorrect subscription status: {stripeSubscription.Status}");
            return;
        }

        // Set the customer's billing plan to the subscription's plan.
        string stripeProductId = stripeSubscription.Items.Data[0].Price.ProductId;
        var newBillingPlan = _stripeService.GetBillingPlanForStripeProduct(stripeProductId);

        _ = newBillingPlan ?? throw new InternalServerException($"Billing plan for product: {stripeProductId} not found.");

        await SetCustomerBillingPlan(invoice.CustomerId, invoice.SubscriptionId, (BillingPlan)newBillingPlan, ct);
    }

    private async Task HandlePriceChanged(Price price, CancellationToken ct)
    {
        await _stripeService.UpsertPrice(price, ct);
        _logger.LogInformation($"Price: {price.Id} data was updated.");
    }

    private async Task HandleProductChanged(Product product, CancellationToken ct)
    {
        await _stripeService.UpsertProduct(product, ct);
        _logger.LogInformation($"Product: {product.Id} data was updated.");
    }

    private async Task SetCustomerBillingPlan(
        string customerId,
        string subscriptionId,
        BillingPlan newBillingPlan,
        CancellationToken ct)
    {
        var spec = new CustomerByStripeCustomerIdSpec(customerId);
        var customer = await _customerRepository.GetBySpecAsync(spec, ct);

        _ = customer ?? throw new InternalServerException(
            $"Customer: {customerId} not found. Was unable to set his billing plan to: {newBillingPlan}.");

        var oldBillingPlan = customer.BillingPlan;

        customer.SetBillingPlan(newBillingPlan);
        customer.SetCurrentSubscription(subscriptionId);
        await _customerRepository.UpdateAsync(customer, ct);

        _logger.LogInformation($"Customer: {customer.Id} billing plan was changed from: {oldBillingPlan} to: {newBillingPlan}.");
    }

    private async Task<bool> WasEventAlreadyProcessed(string eventId, CancellationToken ct)
    {
        return await _eventRepository.GetByIdAsync(eventId, ct) is not null;
    }

    private async Task StoreProcessedEvent(Event ev, CancellationToken ct)
    {
        var stripeEvent = new StripeEvent(ev.Id, ev.Account, ev.ApiVersion, ev.Created);
        await _eventRepository.AddAsync(stripeEvent, ct);
    }
}