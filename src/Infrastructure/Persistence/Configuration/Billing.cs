using FSH.WebApi.Domain.Billing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.WebApi.Infrastructure.Persistence.Configuration;

public class CustomerConfig : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers", SchemaNames.Billing);

        builder.HasKey(c => c.Id);
        builder.Property(c => c.UserId).IsRequired();
        builder.Property(c => c.StripeCustomerId).HasMaxLength(255).IsRequired();
        builder.Property(c => c.BillingPlan).IsRequired().HasDefaultValue(BillingPlan.Free);
        builder.Property(c => c.MonthlyNumberOfInquiriesSent).IsRequired().HasDefaultValue(0);

        builder
            .HasOne(c => c.CurrentSubscription)
            .WithMany()
            .HasForeignKey(c => c.CurrentSubscriptionId)
            .IsRequired(false);

        builder
            .HasMany(c => c.Subscriptions)
            .WithOne(s => s.Customer)
            .HasForeignKey(s => s.CustomerId)
            .HasPrincipalKey(c => c.StripeCustomerId);

        builder.HasIndex(c => c.UserId).IsUnique(true);
        builder.HasIndex(c => c.StripeCustomerId).IsUnique(true);
    }
}

public class StripeSubscriptionConfig : IEntityTypeConfiguration<StripeSubscription>
{
    public void Configure(EntityTypeBuilder<StripeSubscription> builder)
    {
        builder.ToTable("StripeSubscriptions", SchemaNames.Billing);

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasMaxLength(255);
        builder.Property(s => s.CustomerId).IsRequired();
        builder.Property(s => s.Status).IsRequired();
        builder.Property(s => s.PriceId).IsRequired();
        builder.Property(s => s.CancelAtPeriodEnd).IsRequired();
        builder.Property(s => s.CancelAt).IsRequired(false);
        builder.Property(s => s.CanceledAt).IsRequired(false);
        builder.Property(s => s.CollectionMethod).IsRequired();
        builder.Property(s => s.Created).IsRequired();
        builder.Property(s => s.Currency).IsRequired();
        builder.Property(s => s.CurrentPeriodEnd).IsRequired();
        builder.Property(s => s.CurrentPeriodStart).IsRequired();
        builder.Property(s => s.StartDate).IsRequired();
        builder.Property(s => s.EndedAt).IsRequired(false);
        builder.Property(s => s.TrialStart).IsRequired(false);
        builder.Property(s => s.TrialEnd).IsRequired(false);
        builder.Property(s => s.Livemode).IsRequired();

        builder
            .HasOne(s => s.Price)
            .WithMany()
            .HasForeignKey(s => s.PriceId);
    }
}

public class StripeProductConfig : IEntityTypeConfiguration<StripeProduct>
{
    public void Configure(EntityTypeBuilder<StripeProduct> builder)
    {
        builder.ToTable("StripeProducts", SchemaNames.Billing);

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasMaxLength(255);
        builder.Property(p => p.Name).IsRequired();
        builder.Property(p => p.Description).IsRequired(false);
        builder.Property(p => p.IsActive).IsRequired();
        builder.Property(p => p.Livemode).IsRequired();
        builder.Property(p => p.Metadata).HasColumnType("jsonb").IsRequired();

        builder
            .HasMany(product => product.Prices)
            .WithOne()
            .HasForeignKey(price => price.ProductId);
    }
}

public class StripePriceConfig : IEntityTypeConfiguration<StripePrice>
{
    public void Configure(EntityTypeBuilder<StripePrice> builder)
    {
        builder.ToTable("StripePrices", SchemaNames.Billing);

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasMaxLength(255);
        builder.Property(p => p.ProductId).HasMaxLength(255).IsRequired();
        builder.Property(p => p.Type).HasMaxLength(50).IsRequired();
        builder.Property(p => p.Description).HasMaxLength(255).IsRequired(false);
        builder.Property(p => p.UnitAmount).IsRequired(false);
        builder.Property(p => p.UnitAmountDecimal).IsRequired(false);
        builder.Property(p => p.Currency).HasMaxLength(3).IsRequired();
        builder.Property(p => p.TaxBehavior).HasMaxLength(50).IsRequired();
        builder.Property(p => p.Interval).IsRequired();
        builder.Property(p => p.IntervalCount).IsRequired();
        builder.Property(p => p.TrialPeriodDays).IsRequired(false);
        builder.Property(p => p.IsActive).IsRequired();
        builder.Property(p => p.Livemode).IsRequired();
        builder.Property(p => p.Metadata).HasColumnType("jsonb").IsRequired();
    }
}