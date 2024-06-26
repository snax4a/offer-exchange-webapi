using Dapper;
using FSH.WebApi.Application.Common.Interfaces;
using FSH.WebApi.Application.Common.Persistence;
using FSH.WebApi.Application.Comparison;
using FSH.WebApi.Application.Comparison.DTOs;
using FSH.WebApi.Infrastructure.Persistence.Context;

namespace FSH.WebApi.Infrastructure.Persistence.Repository;

public class ComparisonRepository : IComparisonRepository
{
    private readonly ICurrentUser _currentUser;
    private readonly ApplicationDbContext _dbContext;

    public ComparisonRepository(ICurrentUser currentUser, ApplicationDbContext dbContext) =>
        (_currentUser, _dbContext) = (currentUser, dbContext);

    // Finds all active offers for inquiry product
    public Task<IEnumerable<InquiryProductOfferDto>> GetOffersForInquiryProductAsync(
        Guid productId,
        bool withReplacements,
        bool onlyWithFreebies,
        ProductOffersOrder orderBy,
        CancellationToken ct)
    {
        string sql = @"
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
                ""OfferExchange"".""InquiryProducts"" AS ip
                INNER JOIN ""OfferExchange"".""OfferProducts"" AS op ON ip.""Id"" = op.""InquiryProductId""
                INNER JOIN ""OfferExchange"".""Offers"" AS o ON op.""OfferId"" = o.""Id""
                INNER JOIN ""OfferExchange"".""Traders"" AS t ON o.""TraderId"" = t.""Id""
            WHERE
                ip.""Id"" = @Id
                AND ip.""CreatedBy"" = @UserId
                AND ip.""TenantId"" = @Tenant
                AND (o.""ExpirationDate"" IS NULL OR o.""ExpirationDate"" >= NOW()::DATE)
        ";

        if (!withReplacements)
            sql += @" AND op.""IsReplacement"" = false"; // don't take replacements

        if (onlyWithFreebies)
            sql += @" AND op.""Freebie"" IS NOT NULL"; // take offers with freebie

        if (orderBy == ProductOffersOrder.PriceAsc)
            sql += @" ORDER BY op.""NetPrice"" ASC"; // order by net price asc

        if (orderBy == ProductOffersOrder.PriceDesc)
            sql += @" ORDER BY op.""NetPrice"" DESC"; // order by net price desc

        if (orderBy == ProductOffersOrder.DeliveryDateAsc)
            sql += @" ORDER BY op.""DeliveryDate"" ASC"; // order by delivery date asc

        var parameters = new
        {
            Id = productId,
            UserId = _currentUser.GetUserId(),
            Tenant = _dbContext.TenantInfo.Id
        };

        var command = new CommandDefinition(sql, parameters, cancellationToken: ct);
        return _dbContext.Connection.QueryAsync<InquiryProductOfferDto>(command);
    }

    // Finds the best offers for all inquiry products
    // TODO: On production we should monitor this query
    // and consider creating and clustered index on InquiryProductId and NetPrice, DeliveryDate in the subquery
    public Task<IEnumerable<InquiryProductOfferDto>> GetTheBestOffersForAllProductsFromInquiryAsync(
        Guid inquiryId,
        bool withReplacements,
        ComparisonDecisiveParameter decisiveParameter,
        DateOnly? maxDeliveryDate,
        CancellationToken ct)
    {
        string sql = @"
            SELECT
                ip.""Name"" AS ""ProductName"",
                ip.""Quantity"",
                ip.""PreferredDeliveryDate"",
                op.""Id"" AS ""OfferProductId"",
                o.""ExpirationDate"",
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
                ""OfferExchange"".""InquiryProducts"" AS ip
                INNER JOIN ""OfferExchange"".""OfferProducts"" AS op ON ip.""Id"" = op.""InquiryProductId""
                INNER JOIN ""OfferExchange"".""Offers"" AS o ON op.""OfferId"" = o.""Id""
                INNER JOIN ""OfferExchange"".""Traders"" AS t ON o.""TraderId"" = t.""Id""
            WHERE
                ip.""InquiryId"" = @InquiryId
                AND ip.""CreatedBy"" = @UserId
                AND ip.""TenantId"" = @Tenant
                AND op.""Id"" = (@SubQuery)
        ";

        // This subquery is used to find id of best product offer
        string subQuery = @"
            SELECT subOp.""Id""
            FROM ""OfferExchange"".""OfferProducts"" AS subOp
            INNER JOIN ""OfferExchange"".""Offers"" AS subO ON subOp.""OfferId"" = subO.""Id""
            WHERE subOp. ""InquiryProductId"" = ip.""Id""
            AND(subO.""ExpirationDate"" IS NULL OR subO.""ExpirationDate"" >= NOW()::DATE)
        ";

        if (!withReplacements)
            subQuery += @" AND subOp.""IsReplacement"" = false"; // don't take replacements

        if (maxDeliveryDate is not null)
            subQuery += @" AND subOp.""DeliveryDate"" <= @MaxDeliveryDate"; // don't take offers with delivery date > maxDeliveryDate

        if (decisiveParameter == ComparisonDecisiveParameter.LowestPrice)
            subQuery += @" ORDER BY subOp.""NetPrice"", subOp.""DeliveryDate"" ASC LIMIT 1"; // choose offer with lowest price

        if (decisiveParameter == ComparisonDecisiveParameter.NearestDeliveryDate)
            subQuery += @" ORDER BY subOp.""DeliveryDate"", subOp.""NetPrice"" ASC LIMIT 1"; // choose offer with nearest delivery date

        sql = sql.Replace("@SubQuery", subQuery);

        var parameters = new
        {
            InquiryId = inquiryId,
            UserId = _currentUser.GetUserId(),
            Tenant = _dbContext.TenantInfo.Id,
            MaxDeliveryDate = maxDeliveryDate?.ToDateTime(TimeOnly.MinValue)
        };

        var command = new CommandDefinition(sql, parameters, cancellationToken: ct);
        return _dbContext.Connection.QueryAsync<InquiryProductOfferDto>(command);
    }

    // Finds the best offers for selected inquiry products
    // TODO: On production we should monitor this query
    // and consider creating and clustered index on InquiryProductId and NetPrice, DeliveryDate in the subquery
    public Task<IEnumerable<InquiryProductOfferDto>> GetTheBestOffersForSelectedProductsFromInquiryAsync(
        Guid inquiryId,
        IList<Guid> productIds,
        bool withReplacements,
        ComparisonDecisiveParameter decisiveParameter,
        DateOnly? maxDeliveryDate,
        CancellationToken ct)
    {
        string sql = @"
            SELECT
                ip.""Name"" AS ""ProductName"",
                ip.""Quantity"",
                ip.""PreferredDeliveryDate"",
                op.""Id"" AS ""OfferProductId"",
                o.""ExpirationDate"",
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
                ""OfferExchange"".""InquiryProducts"" AS ip
                INNER JOIN ""OfferExchange"".""OfferProducts"" AS op ON ip.""Id"" = op.""InquiryProductId""
                INNER JOIN ""OfferExchange"".""Offers"" AS o ON op.""OfferId"" = o.""Id""
                INNER JOIN ""OfferExchange"".""Traders"" AS t ON o.""TraderId"" = t.""Id""
            WHERE
                ip.""InquiryId"" = @InquiryId
                AND ip.""Id"" = ANY(@ProductIds)
                AND ip.""CreatedBy"" = @UserId
                AND ip.""TenantId"" = @Tenant
                AND op.""Id"" = (@SubQuery)
        ";

        // This subquery is used to find id of best product offer
        string subQuery = @"
            SELECT subOp.""Id""
            FROM ""OfferExchange"".""OfferProducts"" AS subOp
            INNER JOIN ""OfferExchange"".""Offers"" AS subO ON subOp.""OfferId"" = subO.""Id""
            WHERE subOp. ""InquiryProductId"" = ip.""Id""
            AND(subO.""ExpirationDate"" IS NULL OR subO.""ExpirationDate"" >= NOW()::DATE)
        ";

        if (!withReplacements)
            subQuery += @" AND subOp.""IsReplacement"" = false"; // don't take replacements

        if (maxDeliveryDate is not null)
            subQuery += @" AND subOp.""DeliveryDate"" <= @MaxDeliveryDate"; // don't take offers with delivery date > maxDeliveryDate

        if (decisiveParameter == ComparisonDecisiveParameter.LowestPrice)
            subQuery += @" ORDER BY subOp.""NetPrice"", subOp.""DeliveryDate"" ASC LIMIT 1"; // choose offer with lowest price

        if (decisiveParameter == ComparisonDecisiveParameter.NearestDeliveryDate)
            subQuery += @" ORDER BY subOp.""DeliveryDate"", subOp.""NetPrice"" ASC LIMIT 1"; // choose offer with nearest delivery date

        sql = sql.Replace("@SubQuery", subQuery);

        var parameters = new
        {
            InquiryId = inquiryId,
            UserId = _currentUser.GetUserId(),
            Tenant = _dbContext.TenantInfo.Id,
            ProductIds = productIds,
            MaxDeliveryDate = maxDeliveryDate?.ToDateTime(TimeOnly.MinValue)
        };

        var command = new CommandDefinition(sql, parameters, cancellationToken: ct);
        return _dbContext.Connection.QueryAsync<InquiryProductOfferDto>(command);
    }
}