namespace App.Core.Persistence.Configurations.Database.ModelConfigurations.Identity
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using App.Core.Persistence.Identity.Users.Entities;
    using App.Core.Persistence.Configurations.Database.ModelConfigurations.Extensions;

    public class UserModelConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User), "Identity");

            // Apply Common TrackedEntity Mapping (Id, Uid, CreatedOn, UpdatedOn & Uid Index)
            builder.ConfigureTrackedEntity();

            // User Specific Properties (PostgreSQL Specific Types)
            builder.Property(x => x.Email)
                   .HasColumnName("Email")
                   .HasColumnType("character varying")
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(x => x.Password)
                   .HasColumnName("Password")
                   .HasColumnType("character varying")
                   .HasMaxLength(500)
                   .IsRequired();

            builder.Property(x => x.FirstName)
                   .HasColumnName("FirstName")
                   .HasColumnType("character varying")
                   .HasMaxLength(100)
                   .HasDefaultValue(null);

            builder.Property(x => x.LastName)
                   .HasColumnName("LastName")
                   .HasColumnType("character varying")
                   .HasMaxLength(100)
                   .HasDefaultValue(null);

            builder.Property(x => x.Phone)
                   .HasColumnName("Phone")
                   .HasColumnType("character varying")
                   .HasMaxLength(20)
                   .HasDefaultValue(null);

            builder.Property(x => x.IsPhoneVerified)
                   .HasColumnName("IsPhoneVerified")
                   .HasColumnType("boolean")
                   .IsRequired();

            builder.Property(x => x.IsActive)
                   .HasColumnName("IsActive")
                   .HasColumnType("boolean")
                   .IsRequired();

            builder.Property(x => x.IsEmailVerified)
                   .HasColumnName("IsEmailVerified")
                   .HasColumnType("boolean")
                   .IsRequired();

            builder.Property(x => x.LastLoginDate)
                   .HasColumnName("LastLoginDate")
                   .HasColumnType("timestamp with time zone")
                   .HasDefaultValue(null);

            builder.Property(x => x.ProfilePhoto)
                   .HasColumnName("ProfilePhoto")
                   .HasColumnType("text")
                   .HasDefaultValue(null);

            // Indexes
            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.Uid).IsUnique();
        }
    }
}