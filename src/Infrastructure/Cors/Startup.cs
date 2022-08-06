using FSH.WebApi.Infrastructure.ClientApp;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.WebApi.Infrastructure.Cors;

internal static class Startup
{
    private const string CorsPolicy = nameof(CorsPolicy);

    internal static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration config)
    {
        var clientAppSettings = config.GetSection(nameof(ClientAppSettings)).Get<ClientAppSettings>();
        var origins = new List<string>();
        if (clientAppSettings.CorsOrigins is not null)
            origins.AddRange(clientAppSettings.CorsOrigins.Split(';', StringSplitOptions.RemoveEmptyEntries));

        return services.AddCors(opt =>
            opt.AddPolicy(CorsPolicy, policy =>
                policy.SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithOrigins(origins.ToArray())));
    }

    internal static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app) =>
        app.UseCors(CorsPolicy);
}