using System.Reflection;
using FSH.WebApi.Application.Common.Interfaces;
using FSH.WebApi.Domain.Exchange;
using FSH.WebApi.Infrastructure.Persistence.Context;
using FSH.WebApi.Infrastructure.Persistence.Initialization;
using Microsoft.Extensions.Logging;

namespace FSH.WebApi.Infrastructure.Exchange.Traders;

public class TraderSeeder : ICustomSeeder
{
    private readonly ISerializerService _serializerService;
    private readonly ApplicationDbContext _db;
    private readonly ILogger<TraderSeeder> _logger;

    public TraderSeeder(ISerializerService serializerService, ILogger<TraderSeeder> logger, ApplicationDbContext db)
    {
        _serializerService = serializerService;
        _logger = logger;
        _db = db;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (!_db.Traders.Any())
        {
            _logger.LogInformation("Started to Seed Traders.");

            // Here you can use your own logic to populate the database.
            // As an example, I am using a JSON file to populate the database.
            string traderData = await File.ReadAllTextAsync(path + "/Exchange/Traders/traders.json", cancellationToken);
            var traders = _serializerService.Deserialize<List<Trader>>(traderData);

            if (traders != null)
            {
                foreach (var trader in traders)
                {
                    await _db.Traders.AddAsync(trader, cancellationToken);
                }
            }

            await _db.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Seeded Traders.");
        }
    }
}