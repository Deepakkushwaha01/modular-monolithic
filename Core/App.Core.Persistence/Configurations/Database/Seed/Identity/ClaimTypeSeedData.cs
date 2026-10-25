
namespace App.Core.Persistence.Configurations.Database.Context.Extensions.Identity.Seeds;

using App.Core.Persistence.Identity.Claims.Entities;
using Microsoft.EntityFrameworkCore;

internal static class ClaimTypeSeedData
{
    private static readonly DateTime SeededOn =
        new(2026, 8, 22, 0, 0, 0, DateTimeKind.Utc);

    internal static void Add(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.Entity<ClaimType>().HasData(
            Create(
                1,
                "Users.Read",
                "View user details.",
                "Users"),

            Create(
                2,
                "Users.Edit",
                "Create and update user information.",
                "Users"),

            Create(
                3,
                "Users.Delete",
                "Delete user accounts.",
                "Users"),

            Create(
                4,
                "Orders.Read",
                "View order details.",
                "Orders"),

            Create(
                5,
                "Orders.Edit",
                "Create and update order information.",
                "Orders"),

            Create(
                6,
                "Orders.Delete",
                "Delete orders.",
                "Orders"
                )
        );
    }

    private static ClaimType Create(
        long id,
        string name,
        string description,
        string module)
    {
        return ClaimType.Create(
            id: id,
            uid: Guid.Parse(
                $"20000000-0000-0000-0000-{Math.Abs(id):D12}"),
            name: name,
            description: description,
            dataType: ClaimDataType.Boolean,
            module: module,
            isActive: true,
            createdOn: SeededOn,
            updatedOn: null);
    }
}
