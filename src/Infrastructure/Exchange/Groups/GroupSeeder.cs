using System.Reflection;
using FSH.WebApi.Application.Common.Interfaces;
using FSH.WebApi.Domain.Exchange;
using FSH.WebApi.Infrastructure.Persistence.Context;
using FSH.WebApi.Infrastructure.Persistence.Initialization;
using Microsoft.Extensions.Logging;

namespace FSH.WebApi.Infrastructure.Exchange.Groups;

public class GroupSeeder : ICustomSeeder
{
    public short Priority { get; } = 3;
    private readonly ISerializerService _serializerService;
    private readonly ApplicationDbContext _db;
    private readonly ILogger<GroupSeeder> _logger;

    public GroupSeeder(ISerializerService serializerService, ILogger<GroupSeeder> logger, ApplicationDbContext db)
    {
        _serializerService = serializerService;
        _logger = logger;
        _db = db;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (!_db.Groups.Any())
        {
            _logger.LogInformation("Started to Seed Groups.");

            // Here you can use your own logic to populate the database.
            // As an example, I am using a JSON file to populate the database.
            string groupData = await File.ReadAllTextAsync(path + "/Exchange/Groups/groups.json", cancellationToken);
            var groups = _serializerService.Deserialize<List<Group>>(groupData);

            if (groups != null)
            {
                foreach (var group in groups)
                {
                    await _db.Groups.AddAsync(group, cancellationToken);
                }
            }

            await _db.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Seeded Groups.");
        }
    }
}