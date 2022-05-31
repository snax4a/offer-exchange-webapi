using System.Reflection;
using FSH.WebApi.Application.Common.Interfaces;
using FSH.WebApi.Domain.Exchange;
using FSH.WebApi.Infrastructure.Persistence.Context;
using FSH.WebApi.Infrastructure.Persistence.Initialization;
using Mapster;
using Microsoft.Extensions.Logging;

namespace FSH.WebApi.Infrastructure.ISOData.CountrySubdivisions;

public class CountrySubdivisionSeeder : ICustomSeeder
{
    public short Priority { get; } = 2;
    private readonly ISerializerService _serializerService;
    private readonly ApplicationDbContext _db;
    private readonly ILogger<CountrySubdivisionSeeder> _logger;

    public CountrySubdivisionSeeder(ISerializerService serializerService, ILogger<CountrySubdivisionSeeder> logger, ApplicationDbContext db)
    {
        _serializerService = serializerService;
        _logger = logger;
        _db = db;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (!_db.CountrySubdivisions.Any())
        {
            _logger.LogInformation("Started to Seed Country Subdivisions.");

            string subdivisionData = await File.ReadAllTextAsync(path + "/ISOData/CountrySubdivisions/country-subdivisions.json", cancellationToken);
            var subdivisions = _serializerService.Deserialize<List<CountrySubdivisionDataDto>>(subdivisionData);

            if (subdivisions != null)
            {
                foreach (var subdivisionDto in subdivisions)
                {
                    await _db.CountrySubdivisions.AddAsync(subdivisionDto.Adapt<CountrySubdivision>(), cancellationToken);
                }
            }

            await _db.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Seeded Country Subdivisions.");
        }
    }
}