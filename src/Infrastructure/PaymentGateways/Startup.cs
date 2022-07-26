using FSH.WebApi.Infrastructure.PaymentGateways.Stripe;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.WebApi.Infrastructure.PaymentGateways;

internal static class Startup
{
    internal static IServiceCollection AddPaymentGateways(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddOptions<StripeSettings>()
            .Bind(config.GetSection(nameof(StripeSettings)))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}