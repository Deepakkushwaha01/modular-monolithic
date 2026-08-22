using App.Core.Persistence.Configurations.Database.ModelConfigurations.Extensions;
using App.Core.Persistence.Identity.Addresses.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Core.Persistence.Configurations.Database.ModelConfigurations.Identity;

public class AddressModelConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable(nameof(Address), "Identity");
        builder.ConfigureTrackedEntity();

        builder.Property(x => x.UserId).HasColumnType("bigint").IsRequired();
        builder.Property(x => x.AddressType).HasColumnType("character varying").HasMaxLength(20).IsRequired();
        builder.Property(x => x.AddressLine1).HasColumnType("character varying").HasMaxLength(255).IsRequired();
        builder.Property(x => x.AddressLine2).HasColumnType("character varying").HasMaxLength(255).HasDefaultValue(null);
        builder.Property(x => x.City).HasColumnType("character varying").HasMaxLength(100).IsRequired();
        builder.Property(x => x.State).HasColumnType("character varying").HasMaxLength(100).IsRequired();
        builder.Property(x => x.ZipCode).HasColumnType("character varying").HasMaxLength(20).IsRequired();
        builder.Property(x => x.Country).HasColumnType("character varying").HasMaxLength(100).IsRequired();
        builder.Property(x => x.IsDefault).HasColumnType("boolean").HasDefaultValue(false).IsRequired();

        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.Uid).IsUnique();
        builder.HasOne(x => x.User).WithMany(x => x.Addresses).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}
