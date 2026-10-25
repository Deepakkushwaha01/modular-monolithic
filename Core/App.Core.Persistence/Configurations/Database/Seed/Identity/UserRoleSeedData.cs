
namespace App.Core.Persistence.Configurations.Database.Context.Extensions.Identity.Seeds;

using Microsoft.EntityFrameworkCore;

internal static class UserRoleSeedData
{
    private static readonly DateTime SeededOn = new(2026, 8, 22, 0, 0, 0, DateTimeKind.Utc);
    internal static void Add(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity(
            "App.Core.Persistence.Identity.Roles.Entities.UserRole")
            .HasData(new
            {
                Id = 1L,
                Uid = Guid.Parse("50000000-0000-0000-0000-000000000001"),
                UserId = 1L,
                RoleId = 5L,
                AssignedAt = SeededOn,
                AssignedBy = (long?)null,
                CreatedOn = SeededOn,
                CreatedBy = "System",
                UpdatedOn = (DateTime?)null
            },
            new
            {
                Id = 2L,
                Uid = Guid.Parse("50000000-0000-0000-0000-000000000002"),
                UserId = 2L,
                RoleId = 2L,
                AssignedAt = SeededOn,
                AssignedBy = (long?)null,
                CreatedOn = SeededOn,
                CreatedBy = "System",
                UpdatedOn = (DateTime?)null
            },
            new
            {
                Id = 3L,
                Uid = Guid.Parse("50000000-0000-0000-0000-000000000003"),
                UserId = 3L,
                RoleId = 3L,
                AssignedAt = SeededOn,
                AssignedBy = (long?)null,
                CreatedOn = SeededOn,
                CreatedBy = "System",
                UpdatedOn = (DateTime?)null
            });
    }
}
