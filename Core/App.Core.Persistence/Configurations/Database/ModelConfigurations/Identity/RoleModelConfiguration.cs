using App.Core.Persistence.Configurations.Database.ModelConfigurations.Extensions;
using App.Core.Persistence.Identity.Roles.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Core.Persistence.Configurations.Database.ModelConfigurations.Identity;

public class RoleModelConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(nameof(Role), "Identity");
        builder.ConfigureTrackedEntity();

        builder.Property(x => x.Name).HasColumnType("character varying").HasMaxLength(50).IsRequired();
        builder.Property(x => x.Slug).HasColumnType("character varying").HasMaxLength(50).IsRequired();
        builder.Property(x => x.Description).HasColumnType("character varying").HasMaxLength(255).HasDefaultValue(null);
        builder.Property(x => x.IsSystemRole).HasColumnType("boolean").HasDefaultValue(false).IsRequired();
        builder.Property(x => x.IsActive).HasColumnType("boolean").HasDefaultValue(true).IsRequired();

        builder.HasIndex(x => x.Slug).IsUnique();
        builder.HasIndex(x => x.Uid).IsUnique();
    }
}
