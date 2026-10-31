
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
                UserId = Guid.Parse("30000000-0000-0000-0000-000000000001"),
                RoleId = Guid.Parse("10000000-0000-0000-0000-000000000005"),
                AssignedAt = SeededOn,
                AssignedBy = (Guid?)null,
                CreatedOn = SeededOn,
                CreatedBy = "System",
                UpdatedOn = (DateTime?)null
            },
            new
            {
                Id = 2L,
                Uid = Guid.Parse("50000000-0000-0000-0000-000000000002"),
                UserId = Guid.Parse("30000000-0000-0000-0000-000000000002"),
                RoleId = Guid.Parse("10000000-0000-0000-0000-000000000002"),
                AssignedAt = SeededOn,
                AssignedBy = (Guid?)null,
                CreatedOn = SeededOn,
                CreatedBy = "System",
                UpdatedOn = (DateTime?)null
            },
            new
            {
                Id = 3L,
                Uid = Guid.Parse("50000000-0000-0000-0000-000000000003"),
                UserId = Guid.Parse("30000000-0000-0000-0000-000000000003"),
                RoleId = Guid.Parse("10000000-0000-0000-0000-000000000003"),
                AssignedAt = SeededOn,
                AssignedBy = (Guid?)null,
                CreatedOn = SeededOn,
                CreatedBy = "System",
                UpdatedOn = (DateTime?)null
            },
            new
            {
                Id = 4L,
                Uid = Guid.Parse("50000000-0000-0000-0000-000000000004"),
                UserId = Guid.Parse("30000000-0000-0000-0000-000000000004"),
                RoleId = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                AssignedAt = SeededOn,
                AssignedBy = (Guid?)null,
                CreatedOn = SeededOn,
                CreatedBy = "System",
                UpdatedOn = (DateTime?)null
            });
    }
}
