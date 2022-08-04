using Finbuckle.MultiTenant;
using FSH.WebApi.Application.Common.Events;
using FSH.WebApi.Application.Common.Interfaces;
using FSH.WebApi.Domain.Billing;
using FSH.WebApi.Domain.Exchange;
using FSH.WebApi.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FSH.WebApi.Infrastructure.Persistence.Context;

public class ApplicationDbContext : BaseDbContext
{
    public ApplicationDbContext(ITenantInfo currentTenant, DbContextOptions options, ICurrentUser currentUser, ISerializerService serializer, IOptions<DatabaseSettings> dbSettings, IEventPublisher events)
        : base(currentTenant, options, currentUser, serializer, dbSettings, events)
    {
    }

    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Trader> Traders => Set<Trader>();
    public DbSet<Inquiry> Inquiries => Set<Inquiry>();
    public DbSet<InquiryProduct> InquiryProducts => Set<InquiryProduct>();
    public DbSet<InquiryRecipient> InquiryRecipients => Set<InquiryRecipient>();
    public DbSet<Offer> Offers => Set<Offer>();
    public DbSet<OfferProduct> OfferProducts => Set<OfferProduct>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderProduct> OrderProducts => Set<OrderProduct>();
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<CountrySubdivision> CountrySubdivisions => Set<CountrySubdivision>();
    public DbSet<UserAddress> UserAddresses => Set<UserAddress>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<StripeSubscription> StripeSubscriptions => Set<StripeSubscription>();
    public DbSet<StripeProduct> StripeProducts => Set<StripeProduct>();
    public DbSet<StripePrice> StripePrices => Set<StripePrice>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(SchemaNames.OfferExchange);
    }
}