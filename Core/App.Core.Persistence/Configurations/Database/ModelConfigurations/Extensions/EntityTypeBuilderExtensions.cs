namespace App.Core.Persistence.Configurations.Database.ModelConfigurations.Extensions
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using App.Core.Persistence.Configurations.Entity;

    public static class EntityTypeBuilderExtensions
    {
        public static EntityTypeBuilder<TEntity> ConfigureTrackedEntity<TEntity>(this EntityTypeBuilder<TEntity> builder)
            where TEntity : TrackedEntity
        {
            builder.HasKey(x => x.Id);

            // PostgreSQL Primary Key (bigint / int8)
            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .HasColumnType("bigint")
                   .IsRequired();

            // PostgreSQL Native UUID Type
            builder.Property(x => x.Uid)
                   .HasColumnName("Uid")
                   .HasColumnType("uuid")
                   .IsRequired();

            // PostgreSQL Native Timestamp with Time Zone (timestamptz)
            builder.Property(x => x.CreatedOn)
                   .HasColumnName("CreatedOn")
                   .HasColumnType("timestamp with time zone")
                   .IsRequired();

            builder.Property(x => x.UpdatedOn)
                   .HasColumnName("UpdatedOn")
                   .HasColumnType("timestamp with time zone")
                   .HasDefaultValue(null);

            builder.HasIndex(x => x.Uid)
                   .IsUnique();

            return builder;
        }
    }
}