using System.Reflection;
using FSH.WebApi.Application.Common.Behaviours;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.WebApi.Application;

public static class Startup
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        return services
            .AddMediatR(assembly)
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>))
            .AddValidatorsFromAssembly(assembly);
    }
}