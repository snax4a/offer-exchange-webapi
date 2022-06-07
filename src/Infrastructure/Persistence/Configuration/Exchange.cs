using Finbuckle.MultiTenant.EntityFrameworkCore;
using FSH.WebApi.Domain.Exchange;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.WebApi.Infrastructure.Persistence.Configuration;

public class GroupConfig : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.Property(g => g.Name).HasMaxLength(32);
        builder.HasIndex(g => new { g.Name, g.CreatedBy });
        builder.IsMultiTenant().AdjustIndexes();
    }
}

public class TraderConfig : IEntityTypeConfiguration<Trader>
{
    public void Configure(EntityTypeBuilder<Trader> builder)
    {
        builder.Property(t => t.FirstName).HasMaxLength(20);
        builder.Property(t => t.LastName).HasMaxLength(20);
        builder.Property(t => t.Email).HasMaxLength(60);
        builder.Property(t => t.CompanyName).HasMaxLength(100).IsRequired(false);
        builder.HasIndex(t => t.CreatedBy);
        builder.IsMultiTenant().AdjustIndexes();
    }
}

public class TraderGroupConfig : IEntityTypeConfiguration<TraderGroup>
{
    public void Configure(EntityTypeBuilder<TraderGroup> builder)
    {
        builder.HasKey(tg => new { tg.TraderId, tg.GroupId });

        builder
            .HasOne(tg => tg.Trader)
            .WithMany(t => t.TraderGroups)
            .HasForeignKey(tg => tg.TraderId);

        builder
            .HasOne(tg => tg.Group)
            .WithMany(g => g.TraderGroups)
            .HasForeignKey(tg => tg.GroupId);

        builder.IsMultiTenant().AdjustIndexes();
    }
}

public class InquiryConfig : IEntityTypeConfiguration<Inquiry>
{
    public void Configure(EntityTypeBuilder<Inquiry> builder)
    {
        builder.Property(i => i.Name).HasMaxLength(60);
        builder.Property(i => i.Title).HasMaxLength(100);

        builder
            .HasOne(i => i.Address)
            .WithMany()
            .HasForeignKey(i => i.AddressId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(i => new { i.ReferenceNumber, i.CreatedBy }).IsUnique(true);
        builder.IsMultiTenant().AdjustIndexes();
    }
}

public class InquiryProductConfig : IEntityTypeConfiguration<InquiryProduct>
{
    public void Configure(EntityTypeBuilder<InquiryProduct> builder)
    {
        builder.Property(p => p.Name).HasMaxLength(100);
        builder.Property(p => p.PreferredDeliveryDate).IsRequired(false);

        builder
            .HasOne<Inquiry>()
            .WithMany(i => i.Products)
            .HasForeignKey(ip => ip.InquiryId)
            .IsRequired(true);

        builder.HasIndex(ip => ip.CreatedBy);
        builder.IsMultiTenant().AdjustIndexes();
    }
}

public class InquiryRecipientConfig : IEntityTypeConfiguration<InquiryRecipient>
{
    public void Configure(EntityTypeBuilder<InquiryRecipient> builder)
    {
        builder.HasKey(ir => new { ir.InquiryId, ir.TraderId });

        builder
            .HasOne(ir => ir.Inquiry)
            .WithMany(i => i.InquiryRecipients)
            .HasForeignKey(ir => ir.InquiryId);

        builder
            .HasOne(ir => ir.Trader)
            .WithMany(t => t.InquiryRecipients)
            .HasForeignKey(ir => ir.TraderId);

        builder.IsMultiTenant().AdjustIndexes();
    }
}

public class OfferProductConfig : IEntityTypeConfiguration<OfferProduct>
{
    public void Configure(EntityTypeBuilder<OfferProduct> builder)
    {
        builder.Property(op => op.CurrencyCode).HasMaxLength(3);
        builder.Property(op => op.VatRate).IsRequired(false);
        builder.Property(op => op.Quantity).IsRequired(true);
        builder.Property(op => op.NetPrice).IsRequired(true);
        builder.Property(op => op.NetValue).IsRequired(true);
        builder.Property(op => op.GrossValue).IsRequired(true);
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

        builder.IsMultiTenant().AdjustIndexes();
    }
}

public class OfferConfig : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.Property(o => o.CurrencyCode).HasMaxLength(3);
        builder.Property(o => o.NetValue).IsRequired(true);
        builder.Property(o => o.GrossValue).IsRequired(true);
        builder.Property(o => o.ExpirationDate).IsRequired(false);
        builder.Property(o => o.Freebie).HasMaxLength(2000).IsRequired(false);
        builder.Property(o => o.CreatedOn).IsRequired(true);

        // Configure DeliveryCost value object as owned entity
        builder.OwnsOne(o => o.DeliveryCost, deliveryCostBuilder =>
        {
            deliveryCostBuilder
                .Property(dc => dc.Type)
                .HasColumnName("DeliveryCostType")
                .IsRequired(true);

            deliveryCostBuilder
                .Property(dc => dc.GrossPrice)
                .HasColumnName("DeliveryCostGrossPrice")
                .IsRequired(true);

            deliveryCostBuilder
                .Property(dc => dc.Description)
                .HasMaxLength(2000)
                .HasColumnName("DeliveryCostDescription")
                .IsRequired(false);
        });

        // Relations
        builder
            .HasOne(o => o.Inquiry)
            .WithMany(i => i.Offers)
            .HasForeignKey(o => o.InquiryId);

        builder
            .HasOne(o => o.Trader)
            .WithMany(t => t.Offers)
            .HasForeignKey(o => o.TraderId);

        // Indexes
        builder.HasIndex(o => o.CreatedOn);
        builder.HasIndex(o => o.UserId).IsUnique(false);
        builder.IsMultiTenant().AdjustIndexes();
    }
}

public class OrderConfig : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(o => o.CurrencyCode).HasMaxLength(3);
        builder.Property(o => o.NetValue).IsRequired(true);
        builder.Property(o => o.GrossValue).IsRequired(true);

        builder.HasIndex(o => o.CreatedBy);
        builder.IsMultiTenant().AdjustIndexes();
    }
}

public class OrderProductConfig : IEntityTypeConfiguration<OrderProduct>
{
    public void Configure(EntityTypeBuilder<OrderProduct> builder)
    {
        builder.HasKey(op => new { op.OrderId, op.OfferProductId });

        builder
            .HasOne(op => op.Order)
            .WithMany(o => o.Products)
            .HasForeignKey(op => op.OrderId);

        builder
            .HasOne(op => op.OfferProduct)
            .WithMany(ofp => ofp.Orders)
            .HasForeignKey(op => op.OfferProductId);

        builder.IsMultiTenant().AdjustIndexes();
    }
}

public class AddressConfig : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.Property(a => a.CountryCode).HasMaxLength(2).IsRequired(true);
        builder.Property(a => a.CountrySubdivisionName).HasMaxLength(100).IsRequired(true);
        builder.Property(a => a.Line1).HasMaxLength(60).IsRequired(true);
        builder.Property(a => a.Line2).HasMaxLength(60).IsRequired(false);
        builder.Property(a => a.PostalCode).HasMaxLength(12).IsRequired(true);
        builder.Property(a => a.Locality).HasMaxLength(60).IsRequired(true);

        builder
            .HasOne(a => a.Country)
            .WithMany()
            .HasForeignKey(a => a.CountryCode);

        builder.IsMultiTenant().AdjustIndexes();
    }
}

public class UserAddressConfig : IEntityTypeConfiguration<UserAddress>
{
    public void Configure(EntityTypeBuilder<UserAddress> builder)
    {
        builder.Property(ua => ua.Name).HasMaxLength(100).IsRequired(true);

        builder.HasIndex(ua => new { ua.CreatedBy, ua.Name }).IsUnique(true);
        builder.IsMultiTenant().AdjustIndexes();

        builder.HasOne(ua => ua.Address);
    }
}