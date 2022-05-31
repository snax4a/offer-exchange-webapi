using FSH.WebApi.Domain.Exchange;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.WebApi.Infrastructure.Persistence.Configuration;

public class CountryConfig : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("Countries", schema: SchemaNames.ISO);

        builder.HasKey(c => c.Alpha2Code);

        builder.Property(c => c.Alpha2Code).HasMaxLength(2).IsRequired(true);
        builder.Property(c => c.Alpha3Code).HasMaxLength(3).IsRequired(true);
        builder.Property(c => c.NumericCode).HasMaxLength(3).IsRequired(true);
        builder.Property(c => c.Name).HasMaxLength(100).IsRequired(true);
        builder.Property(c => c.CallingCodes).HasMaxLength(15).IsRequired(false);
        builder.Property(c => c.CurrencyCode).HasMaxLength(3).IsRequired(false);
        builder.Property(c => c.CurrencyName).HasMaxLength(50).IsRequired(false);
        builder.Property(c => c.CurrencySymbol).HasMaxLength(10).IsRequired(false);
        builder.Property(c => c.Capital).HasMaxLength(50).IsRequired(false);
        builder.Property(c => c.LanguageCodes).HasMaxLength(30).IsRequired(false);

        builder
            .HasMany(c => c.Subdivisions)
            .WithOne()
            .HasForeignKey(cs => cs.CountryAlpha2Code);
    }
}

public class CountrySubdivisionConfig : IEntityTypeConfiguration<CountrySubdivision>
{
    public void Configure(EntityTypeBuilder<CountrySubdivision> builder)
    {
        builder.ToTable("CountrySubdivisions", schema: SchemaNames.ISO);

        builder.HasKey(cs => new { cs.CountryAlpha2Code, cs.Name, cs.Code });

        builder.Property(cs => cs.CountryAlpha2Code).HasMaxLength(2).IsRequired(true);
        builder.Property(cs => cs.Name).HasMaxLength(100).IsRequired(true);
        builder.Property(cs => cs.Code).HasMaxLength(10).IsRequired(true);
    }
}