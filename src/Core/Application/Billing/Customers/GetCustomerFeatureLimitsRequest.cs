using FSH.Webapi.Core.Application.FeatureUsage;
using FSH.WebApi.Application.Exchange.Billing.Customers.DTOs;

namespace FSH.WebApi.Application.Exchange.Billing.Customers;

public class GetCustomerFeatureUsageRequest : IRequest<List<FeatureUsageDetailsDto>>
{
}

public class GetCustomerFeatureUsageRequestHandler : IRequestHandler<GetCustomerFeatureUsageRequest, List<FeatureUsageDetailsDto>>
{
    private readonly IFeatureUsageService _featureUsageService;

    public GetCustomerFeatureUsageRequestHandler(IFeatureUsageService featureUsageService)
    {
        _featureUsageService = featureUsageService;
    }

    public async Task<List<FeatureUsageDetailsDto>> Handle(GetCustomerFeatureUsageRequest request, CancellationToken ct)
    {
        return await _featureUsageService.GetFeatureUsageData();
    }
}