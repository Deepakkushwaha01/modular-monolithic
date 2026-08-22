using App.Core.Persistence.Configurations.Database.ModelConfigurations.Extensions;
using App.Core.Persistence.Identity.Claims.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Core.Persistence.Configurations.Database.ModelConfigurations.Identity;

public class ClaimTypeModelConfiguration : IEntityTypeConfiguration<ClaimType>
{
    public void Configure(EntityTypeBuilder<ClaimType> builder)
    {
        builder.ToTable(nameof(ClaimType), "Identity");
        builder.ConfigureTrackedEntity();

        builder.Property(x => x.Name).HasColumnType("character varying").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Description).HasColumnType("character varying").HasMaxLength(255).HasDefaultValue(null);
        builder.Property(x => x.DataType).HasColumnType("character varying").HasMaxLength(20).HasConversion<string>().HasDefaultValue(ClaimDataType.Boolean).IsRequired();
        builder.Property(x => x.Module).HasColumnType("character varying").HasMaxLength(50).IsRequired();
        builder.Property(x => x.IsActive).HasColumnType("boolean").HasDefaultValue(true).IsRequired();

        builder.HasIndex(x => x.Name).IsUnique();
        builder.HasIndex(x => x.Module);
    }
}
