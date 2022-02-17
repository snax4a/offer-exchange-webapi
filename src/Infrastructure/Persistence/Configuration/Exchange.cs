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