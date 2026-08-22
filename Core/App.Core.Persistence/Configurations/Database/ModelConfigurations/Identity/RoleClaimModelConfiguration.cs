using App.Core.Persistence.Identity.Claims.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using App.Core.Persistence.Configurations.Database.ModelConfigurations.Extensions;

namespace App.Core.Persistence.Configurations.Database.ModelConfigurations.Identity;

public class RoleClaimModelConfiguration : IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.ToTable(nameof(RoleClaim), "Identity");
        builder.HasKey(x => new { x.RoleId, x.ClaimTypeId });
        
        builder.ConfigureTrackedEntity();

        builder.Property(x => x.RoleId).HasColumnType("bigint").IsRequired();
        builder.Property(x => x.ClaimTypeId).HasColumnType("bigint").IsRequired();
        builder.Property(x => x.ClaimValue).HasColumnType("character varying").HasMaxLength(255).HasDefaultValue("true").IsRequired();

        builder.HasOne(x => x.Role).WithMany(x => x.RoleClaims).HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.ClaimType).WithMany(x => x.RoleClaims).HasForeignKey(x => x.ClaimTypeId).OnDelete(DeleteBehavior.Cascade);
    }
}
