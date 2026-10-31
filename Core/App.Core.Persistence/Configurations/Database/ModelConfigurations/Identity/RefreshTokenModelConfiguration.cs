using App.Core.Persistence.Identity.Authentication.Entities;
using App.Core.Persistence.Configurations.Database.ModelConfigurations.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Core.Persistence.Configurations.Database.ModelConfigurations.Identity;

public sealed class RefreshTokenModelConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable(nameof(RefreshToken), "Identity");
        builder.ConfigureTrackedEntity();
        builder.Property(x => x.UserUid).HasColumnType("uuid").IsRequired();
        builder.Property(x => x.FamilyUid).HasColumnType("uuid").IsRequired();
        builder.Property(x => x.TokenHash).HasColumnType("character varying").HasMaxLength(64).IsRequired();
        builder.Property(x => x.ExpiresOn).HasColumnType("timestamp with time zone").IsRequired();
        builder.Property(x => x.RevokedOn).HasColumnType("timestamp with time zone");
        builder.Property(x => x.ReplacedByTokenHash).HasColumnType("character varying").HasMaxLength(64);
        builder.HasIndex(x => x.TokenHash).IsUnique();
        builder.HasIndex(x => new { x.UserUid, x.FamilyUid });
        builder.HasIndex(x => x.ExpiresOn);
        builder.HasOne<App.Core.Persistence.Identity.Users.Entities.User>()
            .WithMany()
            .HasForeignKey(x => x.UserUid)
            .HasPrincipalKey(user => user.Uid)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
