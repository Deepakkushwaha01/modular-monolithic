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
        
        builder.Property(x => x.UserId).HasColumnType("bigint").IsRequired();
        builder.Property(x => x.RoleId).HasColumnType("bigint").IsRequired();
        builder.Property(x => x.AssignedAt).HasColumnType("timestamp with time zone").HasDefaultValueSql("now()").IsRequired();
        builder.Property(x => x.AssignedBy).HasColumnType("bigint").HasDefaultValue(null);

        builder.HasIndex(x => x.RoleId);
        builder.HasOne(x => x.User).WithMany(x => x.UserRoles).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Role).WithMany(x => x.UserRoles).HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.AssignedByUser).WithMany().HasForeignKey(x => x.AssignedBy).OnDelete(DeleteBehavior.SetNull);
    }
}
