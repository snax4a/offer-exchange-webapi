using FSH.WebApi.Application.Exchange.Billing.Stripe;
using FSH.WebApi.Application.Exchange.Billing.Stripe.DTOs;

namespace FSH.WebApi.Host.Controllers.Billing;

public class StripeController : VersionNeutralApiController
{
    [TenantIdHeader]
    [HttpPost("customer-portal-session")]
    [OpenApiOperation("Create Stripe checkout session.", "")]
    public Task<StripeCustomerPortalSessionDto> CreateCustomerPortalSession()
    {
        return Mediator.Send(new CreateCustomerPortalSessionRequest());
    }

    [TenantIdHeader]
    [HttpPost("checkout-session")]
    [OpenApiOperation("Create Stripe checkout session.", "")]
    public Task<StripeCheckoutSessionDto> CreateStripeCheckoutSessionAsync(CreateStripeCheckoutSessionRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost("webhook")]
    [AllowAnonymous]
    [OpenApiOperation("Process Stripe webhook.", "")]
    public async Task<bool> ProcessStripeWebhookAsync()
    {
        string payload = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        string signature = HttpContext.Request.Headers["Stripe-Signature"].ToString();
        return await Mediator.Send(new StripeWebhookRequest(payload, signature));
    }
}