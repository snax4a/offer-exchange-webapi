using FSH.WebApi.Infrastructure.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.WebApi.Infrastructure.ClientApp;

internal static class Startup
{
    internal static IServiceCollection AddClientApp(this IServiceCollection services, IConfiguration config) =>
        services.Configure<ClientAppSettings>(config.GetSection(nameof(ClientAppSettings)));
}