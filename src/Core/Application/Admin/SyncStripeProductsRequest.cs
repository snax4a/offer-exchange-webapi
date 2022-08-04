using FSH.WebApi.Application.Admin.DTOs;
using FSH.WebApi.Application.Billing.Stripe;
using FSH.WebApi.Domain.Billing;

namespace FSH.WebApi.Application.Admin;

public class SyncStripeProductsRequest : IRequest<SyncStripeProductsResponse>
{
}

public class SyncStripeProductsRequestHandler : IRequestHandler<SyncStripeProductsRequest, SyncStripeProductsResponse>
{
    private readonly IStripeService _stripeService;

    public SyncStripeProductsRequestHandler(IStripeService stripeService)
    {
        _stripeService = stripeService;
    }

    public async Task<SyncStripeProductsResponse> Handle(SyncStripeProductsRequest request, CancellationToken ct)
    {
        int productsSynced = 0;
        int pricesSynced = 0;

        var stripeProducts = await _stripeService.ListAllActiveProducts(ct);

        foreach (var product in stripeProducts)
        {
            await _stripeService.UpsertProduct(product, ct);
            productsSynced++;
        }

        var stripePrices = await _stripeService.ListAllActivePrices(ct);

        foreach (var price in stripePrices)
        {
            await _stripeService.UpsertPrice(price, ct);
            pricesSynced++;
        }

        return new SyncStripeProductsResponse()
        {
            ProductsSynced = productsSynced,
            PricesSynced = pricesSynced
        };
    }
}