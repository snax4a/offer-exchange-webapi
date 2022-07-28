using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.WebApi.Infrastructure.FeatureUsage;

internal static class Startup
{
    internal static IServiceCollection AddFeatureUsageLimiter(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddOptions<FeatureUsageLimiterSettings>()
            .Bind(config.GetSection(nameof(FeatureUsageLimiterSettings)))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}