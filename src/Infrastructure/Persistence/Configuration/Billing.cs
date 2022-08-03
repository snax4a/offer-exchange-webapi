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
        builder.Property(c => c.StripeCustomerId).IsRequired();
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
        builder.Property(s => s.CustomerId).IsRequired();
        builder.Property(s => s.Status).IsRequired();
        builder.Property(s => s.PriceId).IsRequired();
        builder.Property(s => s.CancelAtPeriodEnd).IsRequired();
        builder.Property(s => s.CancelAt).IsRequired(false);
        builder.Property(s => s.CanceledAt).IsRequired(false);
        builder.Property(s => s.CollectionMethod).IsRequired(true);
        builder.Property(s => s.Created).IsRequired(true);
        builder.Property(s => s.Currency).IsRequired(true);
        builder.Property(s => s.CurrentPeriodEnd).IsRequired(true);
        builder.Property(s => s.CurrentPeriodStart).IsRequired(true);
        builder.Property(s => s.StartDate).IsRequired(true);
        builder.Property(s => s.EndedAt).IsRequired(false);
        builder.Property(s => s.TrialStart).IsRequired(false);
        builder.Property(s => s.TrialEnd).IsRequired(false);
        builder.Property(s => s.Livemode).IsRequired(true);
    }
}