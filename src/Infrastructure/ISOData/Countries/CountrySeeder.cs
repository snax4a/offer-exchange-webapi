using System.Reflection;
using FSH.WebApi.Application.Common.Interfaces;
using FSH.WebApi.Domain.Exchange;
using FSH.WebApi.Infrastructure.Persistence.Context;
using FSH.WebApi.Infrastructure.Persistence.Initialization;
using Mapster;
using Microsoft.Extensions.Logging;

namespace FSH.WebApi.Infrastructure.ISOData.Countries;

public class CountrySeeder : ICustomSeeder
{
    public short Priority { get; } = 1;
    private readonly ISerializerService _serializerService;
    private readonly ApplicationDbContext _db;
    private readonly ILogger<CountrySeeder> _logger;

    public CountrySeeder(ISerializerService serializerService, ILogger<CountrySeeder> logger, ApplicationDbContext db)
    {
        _serializerService = serializerService;
        _logger = logger;
        _db = db;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (!_db.Countries.Any())
        {
            _logger.LogInformation("Started to Seed Countries.");

            string countryData = await File.ReadAllTextAsync(path + "/ISOData/Countries/countries.json", cancellationToken);
            var countries = _serializerService.Deserialize<List<CountryDataDto>>(countryData);

            if (countries != null)
            {
                foreach (var countryDto in countries)
                {
                    await _db.Countries.AddAsync(countryDto.Adapt<Country>(), cancellationToken);
                }
            }

            await _db.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Seeded Country.");
        }
    }
}