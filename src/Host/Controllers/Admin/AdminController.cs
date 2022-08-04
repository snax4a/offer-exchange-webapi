using FSH.WebApi.Application.Admin;
using FSH.WebApi.Application.Admin.DTOs;

namespace FSH.WebApi.Host.Controllers.Admin;

public class AdminController : VersionedApiController
{
    [HttpPost("create-missing-customers")]
    [MustHavePermission(ResourceAction.Manage, Resource.Customers)]
    [OpenApiOperation("Create missing customers for users who don't have.", "")]
    public Task<CreateCustomersResponse> CreateCustomersAsync()
    {
        return Mediator.Send(new CreateCustomersRequest());
    }

    [HttpPost("sync-stripe-products")]
    [MustHavePermission(ResourceAction.Manage, Resource.Customers)]
    [OpenApiOperation("Sync stripe products and prices data.", "")]
    public Task<SyncStripeProductsResponse> SyncProductsAsync()
    {
        return Mediator.Send(new SyncStripeProductsRequest());
    }
}