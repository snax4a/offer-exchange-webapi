using Dapper;
using FSH.WebApi.Application.FeatureUsage;
using FSH.WebApi.Infrastructure.Persistence.Context;
using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.Logging;

namespace FSH.WebApi.Infrastructure.FeatureUsage;

public class MonthlyFeatureUsageJob : IMonthlyFeatureUsageJob
{
    private readonly PerformingContext _performingContext;
    private readonly ILogger<MonthlyFeatureUsageJob> _logger;
    private readonly ApplicationDbContext _dbContext;

    public MonthlyFeatureUsageJob(
        PerformingContext performingContext,
        ILogger<MonthlyFeatureUsageJob> logger,
        ApplicationDbContext dbContext)
    {
        _logger = logger;
        _performingContext = performingContext;
        _dbContext = dbContext;
    }

    [Queue("default")]
    [AutomaticRetry(Attempts = 15)]
    public async Task ResetAllCustomersUsageDataAsync()
    {
        _logger.LogInformation("Started ResetAllCustomersUsageData job with Id: {jobId}", _performingContext.BackgroundJob.Id);

        // Reset customers MonthlyNumberOfInquiriesSent
        const string sql = @"UPDATE ""Billing"".""Customers"" SET ""MonthlyNumberOfInquiriesSent"" = 0";
        int rowsAffected = await _dbContext.Connection.ExecuteAsync(sql);

        _logger.LogInformation($"{rowsAffected} customers monthly feature usage data has been successfully reset.");
    }
}