using FSH.WebApi.Infrastructure.Multitenancy;

namespace FSH.WebApi.Infrastructure.Persistence.Initialization;

internal interface IDatabaseInitializer
{
    Task InitializeDatabasesAsync(CancellationToken cancellationToken);
    Task InitializeApplicationDbForTenantAsync(AppTenantInfo tenant, CancellationToken cancellationToken);
}