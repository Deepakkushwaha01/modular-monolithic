using App.Core.Persistence.Configurations.Database.ModelConfigurations.Extensions;
using App.Core.Persistence.Identity.Claims.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Core.Persistence.Configurations.Database.ModelConfigurations.Identity;

public class UserClaimModelConfiguration : IEntityTypeConfiguration<UserClaim>
{
    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        builder.ToTable(nameof(UserClaim), "Identity");
        builder.ConfigureTrackedEntity();
        
        builder.HasKey(x => new { x.UserId, x.ClaimTypeId });
        builder.Property(x => x.UserId).HasColumnType("bigint").IsRequired();
        builder.Property(x => x.ClaimTypeId).HasColumnType("bigint").IsRequired();
        builder.Property(x => x.ClaimValue).HasColumnType("character varying").HasMaxLength(255).HasDefaultValue("true").IsRequired();

        builder.HasOne(x => x.User).WithMany(x => x.UserClaims).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.ClaimType).WithMany(x => x.UserClaims).HasForeignKey(x => x.ClaimTypeId).OnDelete(DeleteBehavior.Cascade);
    }
}
