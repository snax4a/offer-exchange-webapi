namespace FSH.WebApi.Infrastructure.Persistence.Initialization;

public interface ICustomSeeder
{
    public short Priority { get; } // Used to order seeders
    Task InitializeAsync(CancellationToken cancellationToken);
}