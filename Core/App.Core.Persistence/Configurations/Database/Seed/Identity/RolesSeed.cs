
namespace App.Core.Persistence.Configurations.Database.Context.Extensions.Identity.Seeds;

using Microsoft.EntityFrameworkCore;

internal static class RoleSeedData
{
    private static readonly DateTime SeededOn = new(2026, 8, 22, 0, 0, 0, DateTimeKind.Utc);
    internal static void Add(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity(
            "App.Core.Persistence.Identity.Roles.Entities.Role")
            .HasData(
                new
                {
                    Id = 1L,
                    Uid = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                    Name = "SuperAdmin",
                    Slug = "super-admin",
                    Description = "Full access to store operations, configuration, and access control.",
                    IsSystemRole = true,
                    IsActive = true,
                    CreatedOn = SeededOn,
                    CreatedBy = "System",
                    UpdatedOn = (DateTime?)null
                },
                new
                {
                    Id = 2L,
                    Uid = Guid.Parse("10000000-0000-0000-0000-000000000002"),
                    Name = "Admin",
                    Slug = "admin",
                    Description = "Store administration, catalog, orders, customers, and reports.",
                    IsSystemRole = true,
                    IsActive = true,
                    CreatedOn = SeededOn,
                    CreatedBy = "System",
                    UpdatedOn = (DateTime?)null
                },
                new
                {
                    Id = 3L,
                    Uid = Guid.Parse("10000000-0000-0000-0000-000000000003"),
                    Name = "Manager",
                    Slug = "manager",
                    Description = "Day-to-day store, inventory, fulfillment, and customer service operations.",
                    IsSystemRole = true,
                    IsActive = true,
                    CreatedOn = SeededOn,
                    CreatedBy = "System",
                    UpdatedOn = (DateTime?)null
                },
                new
                {
                    Id = 4L,
                    Uid = Guid.Parse("10000000-0000-0000-0000-000000000004"),
                    Name = "Employee",
                    Slug = "employee",
                    Description = "Limited staff access to assigned store operations.",
                    IsSystemRole = true,
                    IsActive = true,
                    CreatedOn = SeededOn,
                    CreatedBy = "System",
                    UpdatedOn = (DateTime?)null
                },
                new
                {
                    Id = 5L,
                    Uid = Guid.Parse("10000000-0000-0000-0000-000000000005"),
                    Name = "User",
                    Slug = "user",
                    Description = "Default storefront customer access.",
                    IsSystemRole = true,
                    IsActive = true,
                    CreatedOn = SeededOn,
                    CreatedBy = "System",
                    UpdatedOn = (DateTime?)null
                });
    }
}
