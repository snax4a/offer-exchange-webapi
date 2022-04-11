using Dapper;
using FSH.WebApi.Application.Common.Persistence;
using FSH.WebApi.Application.Comparison.DTOs;
using FSH.WebApi.Infrastructure.Persistence.Context;

namespace FSH.WebApi.Infrastructure.Persistence.Repository;

public class ComparisonRepository : IComparisonRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ComparisonRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    // Finds all active offers for inquiry product
    public Task<IEnumerable<InquiryProductOfferDto>> GetOffersForInquiryProductAsync(Guid productId, Guid userId, CancellationToken ct)
    {
        const string sql = @"
            SELECT
                ip.""Name"" AS ""ProductName"",
                ip.""Quantity"",
                ip.""PreferredDeliveryDate"",
                op.""Id"" AS ""OfferProductId"",
                op.""CurrencyCode"",
                op.""DeliveryDate"",
                op.""Freebie"",
                op.""IsReplacement"",
                op.""ReplacementName"",
                op.""NetPrice"",
                op.""NetValue"",
                op.""GrossValue"",
                op.""VatRate"",
                t.""Id"" AS ""TraderId"",
                t.""FirstName"" || ' ' || t.""LastName"" AS ""TraderFullName"",
                t.""Email"" AS ""TraderEmail""
            FROM
                ""Catalog"".""InquiryProducts"" AS ip
                INNER JOIN ""Catalog"".""OfferProducts"" AS op ON ip.""Id"" = op.""InquiryProductId""
                INNER JOIN ""Catalog"".""Offers"" AS o ON op.""OfferId"" = o.""Id""
                INNER JOIN ""Catalog"".""Traders"" AS t ON o.""TraderId"" = t.""Id""
            WHERE
                ip.""Id"" = @Id
                AND ip.""CreatedBy"" = @UserId
                AND ip.""TenantId"" = @Tenant
                AND o.""ExpirationDate"" IS NULL OR o.""ExpirationDate"" >= NOW()::DATE
        ";

        var parameters = new
        {
            Id = productId,
            UserId = userId,
            Tenant = _dbContext.TenantInfo.Id
        };

        var command = new CommandDefinition(sql, parameters, cancellationToken: ct);
        return _dbContext.Connection.QueryAsync<InquiryProductOfferDto>(command);
    }
}