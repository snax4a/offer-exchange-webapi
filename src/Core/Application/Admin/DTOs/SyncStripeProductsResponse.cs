namespace FSH.WebApi.Application.Admin.DTOs;

public class SyncStripeProductsResponse
{
    public int ProductsSynced { get; set; }
    public int PricesSynced { get; set; }
}