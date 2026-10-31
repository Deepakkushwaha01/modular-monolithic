using App.Core.Persistence.Identity.Roles.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using App.Core.Persistence.Configurations.Database.ModelConfigurations.Extensions;

namespace App.Core.Persistence.Configurations.Database.ModelConfigurations.Identity;

public class UserRoleModelConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable(nameof(UserRole), "Identity");
        builder.HasKey(x => new { x.UserId, x.RoleId });

        builder.ConfigureTrackedEntity();

        builder.Property(x => x.UserId).HasColumnType("uuid").IsRequired();
        builder.Property(x => x.RoleId).HasColumnType("uuid").IsRequired();
        builder.Property(x => x.AssignedAt).HasColumnType("timestamp with time zone").HasDefaultValueSql("now()").IsRequired();
        builder.Property(x => x.AssignedBy).HasColumnType("uuid").HasDefaultValue(null);

        builder.HasIndex(x => x.RoleId);
        builder.HasOne(x => x.User).WithMany(x => x.UserRoles).HasForeignKey(x => x.UserId).HasPrincipalKey(x => x.Uid).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Role).WithMany(x => x.UserRoles).HasForeignKey(x => x.RoleId).HasPrincipalKey(x => x.Uid).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.AssignedByUser).WithMany().HasForeignKey(x => x.AssignedBy).HasPrincipalKey(x => x.Uid).OnDelete(DeleteBehavior.SetNull);
    }
}
