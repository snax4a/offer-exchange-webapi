using FSH.Webapi.Core.Application.FeatureUsage;
using FSH.WebApi.Application.Common.Interfaces;
using FSH.WebApi.Application.Common.Persistence;
using FSH.WebApi.Application.Exchange.Billing.Customers.Specifications;
using FSH.WebApi.Core.Shared.FeatureUsage;
using FSH.WebApi.Domain.Billing;
using Hangfire;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FSH.WebApi.Infrastructure.FeatureUsage;

// Currently we are storing usage data in the database.
// In future we might consider storing it in a s3 bucket as a key-value pairs.
public class FeatureUsageService : IFeatureUsageService
{
    private readonly ILogger<FeatureUsageService> _logger;
    private readonly FeatureUsageLimiterSettings _limiterSettings;
    private readonly IRepositoryWithEvents<Customer> _customerRepository;
    private readonly ICurrentUser _currentUser;
    private Customer? _customer; // Cached customer object.

    public FeatureUsageService(
        ILogger<FeatureUsageService> logger,
        IOptions<FeatureUsageLimiterSettings> limiterSettings,
        IRepositoryWithEvents<Customer> customerRepository,
        ICurrentUser currentUser)
    {
        _logger = logger;
        _limiterSettings = limiterSettings.Value;
        _customerRepository = customerRepository;
        _currentUser = currentUser;
    }

    public static void RegisterRecurringJobs()
    {
        // Register monthly feature usage reset job
        RecurringJob.AddOrUpdate<MonthlyFeatureUsageJob>(
            "reset-monthly-fature-usage", x => x.ResetAllCustomersUsageDataAsync(), Cron.Monthly);
    }

    // Check if user has not exceeded the limit for the feature.
    public async Task<bool> CanUseFeature(AppFeatureIds featureId, short? valueToCheck)
    {
        if (!_limiterSettings.EnableLimiter) return true;

        var customer = await GetCustomer();
        var featureLimit = GetFeatureLimit(featureId, customer.BillingPlan);
        short featureLimitValue = featureLimit?.Value ?? short.MaxValue;
        short featureUsage = await GetFeatureUsage(featureId);

        return featureUsage < featureLimitValue;
    }

    // Get feature limit for given billing plan from limiter settings.
    public FeatureLimit? GetFeatureLimit(AppFeatureIds featureId, BillingPlan billingPlan)
    {
        return _limiterSettings.Plans
            .Where(p => p.PlanId == billingPlan)
            .SelectMany(p => p.FeatureLimits)
            .FirstOrDefault(f => f.FeatureId == featureId);
    }

    public async Task<short> GetFeatureUsage(AppFeatureIds featureId)
    {
        try
        {
            switch (featureId)
            {
                case AppFeatureIds.Inquiries_MonthlyCount:
                    return await GetInquiriesSent();
                default:
                    _logger.LogWarning($"Getting usage for feature {featureId} is not supported.");
                    return 0;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting usage for {FeatureId}", featureId);
        }

        return 0;
    }

    public async Task IncrementUsage(AppFeatureIds featureId)
    {
        if (!_limiterSettings.EnableLimiter) return;

        var userId = _currentUser.GetUserId();

        try
        {
            switch (featureId)
            {
                case AppFeatureIds.Inquiries_MonthlyCount:
                    await IncrementInquiriesSent();
                    break;
                default:
                    _logger.LogWarning($"Incrementing usage for feature {featureId} is not supported.");
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while incrementing usage for feature: {FeatureId}, userId: {UserId}", featureId, userId);
        }
    }

    public async Task DecrementUsage(AppFeatureIds featureId)
    {
        if (!_limiterSettings.EnableLimiter) return;

        var userId = _currentUser.GetUserId();

        try
        {
            switch (featureId)
            {
                case AppFeatureIds.Inquiries_MonthlyCount:
                    await DecrementInquiriesSent();
                    break;
                default:
                    _logger.LogWarning($"Decrementing usage for feature {featureId} is not supported.");
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while decrementing usage for feature: {FeatureId}, userId: {UserId}", featureId, userId);
        }
    }

    public async Task SetUsage(AppFeatureIds featureId, short newValue)
    {
        if (!_limiterSettings.EnableLimiter) return;

        var userId = _currentUser.GetUserId();

        try
        {
            switch (featureId)
            {
                case AppFeatureIds.Inquiries_MonthlyCount:
                    await SetInquiriesSent(newValue);
                    break;
                default:
                    _logger.LogWarning($"Setting usage for feature {featureId} is not supported.");
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while setting usage for feature: {FeatureId}, userId: {UserId}", featureId, userId);
        }
    }

    private async Task IncrementInquiriesSent()
    {
        var customer = await GetCustomer();
        customer.IncrementMonthlyNumberOfInquiriesSent();
        await _customerRepository.UpdateAsync(customer);
    }

    private async Task DecrementInquiriesSent()
    {
        var customer = await GetCustomer();
        customer.DecrementMonthlyNumberOfInquiriesSent();
        await _customerRepository.UpdateAsync(customer);
    }

    private async Task SetInquiriesSent(short newValue)
    {
        var customer = await GetCustomer();
        customer.SetMonthlyNumberOfInquiriesSent(newValue);
        await _customerRepository.UpdateAsync(customer);
    }

    private async Task<short> GetInquiriesSent()
    {
        var customer = await GetCustomer();
        return customer.MonthlyNumberOfInquiriesSent;
    }

    // Get customer from cache if exists otherwise query the database and cache it.
    private async Task<Customer> GetCustomer()
    {
        if (_customer != null) return _customer;
        var userId = _currentUser.GetUserId();
        var spec = new CustomerByUserIdSpec(userId);
        var customer = await _customerRepository.GetBySpecAsync(spec);
        _customer = customer ?? throw new InvalidOperationException($"Customer with userId: {userId} not found.");
        return _customer;
    }
}