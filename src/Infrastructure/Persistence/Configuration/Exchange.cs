using Finbuckle.MultiTenant.EntityFrameworkCore;
using FSH.WebApi.Domain.Exchange;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.WebApi.Infrastructure.Persistence.Configuration;

public class GroupConfig : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.IsMultiTenant();
        builder.Property(g => g.Name).HasMaxLength(32);
        builder.HasIndex(g => new { g.Name, g.CreatedBy }).IsUnique(true);
    }
}

public class TraderConfig : IEntityTypeConfiguration<Trader>
{
    public void Configure(EntityTypeBuilder<Trader> builder)
    {
        builder.IsMultiTenant();
        builder.Property(t => t.FirstName).HasMaxLength(20);
        builder.Property(t => t.LastName).HasMaxLength(20);
        builder.Property(t => t.Email).HasMaxLength(60);
    }
}

public class TraderGroupConfig : IEntityTypeConfiguration<TraderGroup>
{
    public void Configure(EntityTypeBuilder<TraderGroup> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(tg => new { tg.TraderId, tg.GroupId });

        builder
            .HasOne(tg => tg.Trader)
            .WithMany(t => t.TraderGroups)
            .HasForeignKey(tg => tg.TraderId);

        builder
            .HasOne(tg => tg.Group)
            .WithMany(g => g.TraderGroups)
            .HasForeignKey(tg => tg.GroupId);
    }
}

public class InquiryProductConfig : IEntityTypeConfiguration<InquiryProduct>
{
    public void Configure(EntityTypeBuilder<InquiryProduct> builder)
    {
        builder.IsMultiTenant();
        builder.Property(p => p.Name).HasMaxLength(100);

        builder
            .HasOne(ip => ip.Inquiry)
            .WithMany(i => i.Products)
            .HasForeignKey(ip => ip.InquiryId);
    }
}

public class InquiryRecipientConfig : IEntityTypeConfiguration<InquiryRecipient>
{
    public void Configure(EntityTypeBuilder<InquiryRecipient> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(ir => new { ir.InquiryId, ir.TraderId });

        builder
            .HasOne(ir => ir.Inquiry)
            .WithMany(i => i.InquiryRecipients)
            .HasForeignKey(ir => ir.InquiryId);

        builder
            .HasOne(ir => ir.Trader)
            .WithMany(t => t.InquiryRecipients)
            .HasForeignKey(ir => ir.TraderId);
    }
}

public class OfferProductConfig : IEntityTypeConfiguration<OfferProduct>
{
    public void Configure(EntityTypeBuilder<OfferProduct> builder)
    {
        builder.IsMultiTenant();
        builder.Property(op => op.CurrencyCode).HasMaxLength(3);
        builder.Property(op => op.NetPrice).HasColumnType("decimal(3,2)");
        builder.Property(op => op.ReplacementName).IsRequired(false).HasMaxLength(100);
        builder.Property(op => op.Freebie).HasMaxLength(2000).IsRequired(false);

        builder
            .HasOne(op => op.Offer)
            .WithMany(o => o.OfferProducts)
            .HasForeignKey(op => op.OfferId);

        builder
            .HasOne(op => op.InquiryProduct)
            .WithMany(ip => ip.OfferProducts)
            .HasForeignKey(op => op.InquiryProductId);
    }
}

public class InquiryConfig : IEntityTypeConfiguration<Inquiry>
{
    public void Configure(EntityTypeBuilder<Inquiry> builder)
    {
        builder.IsMultiTenant();
        builder.HasIndex(i => new { i.ReferenceNumber, i.CreatedBy }).IsUnique(true);
        builder.Property(i => i.Name).HasMaxLength(60);
        builder.Property(i => i.Title).HasMaxLength(100);
    }
}

public class OfferConfig : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.IsMultiTenant();
        builder.Property(o => o.CurrencyCode).HasMaxLength(3);
        builder.Property(o => o.NetValue).HasColumnType("decimal(18,2)");
        builder.Property(o => o.GrossValue).HasColumnType("decimal(18,2)");
        builder.Property(o => o.Freebie).HasMaxLength(2000).IsRequired(false);

        // Configure DeliveryCost value object as owned entity
        builder.OwnsOne(o => o.DeliveryCost, deliveryCostBuilder =>
        {
            deliveryCostBuilder
                .Property(dc => dc.Type)
                .HasColumnName("DeliveryCostType")
                .IsRequired(true);

            deliveryCostBuilder
                .Property(dc => dc.GrossPrice)
                .HasColumnType("decimal(18,2)")
                .HasColumnName("DeliveryCostGrossPrice")
                .IsRequired(false);

            deliveryCostBuilder
                .Property(dc => dc.Description)
                .HasMaxLength(2000)
                .HasColumnName("DeliveryCostDescription")
                .IsRequired(false);
        });

        builder
            .HasOne(o => o.Inquiry)
            .WithMany(i => i.Offers)
            .HasForeignKey(o => o.InquiryId);

        builder
            .HasOne(o => o.Trader)
            .WithMany(t => t.Offers)
            .HasForeignKey(o => o.TraderId);
    }
}