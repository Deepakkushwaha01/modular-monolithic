
namespace App.Core.Persistence.Configurations.Database.Context.Extensions.Identity.Seeds;

using Microsoft.EntityFrameworkCore;

internal static class RoleClaimSeedData
{
    // Claim IDs correspond to ClaimTypeSeedData:
    // 1 Users.Read
    // 2 Users.Edit
    // 3 Users.Delete
    // 4 Orders.Read
    // 5 Orders.Edit
    // 6 Orders.Delete
    // 8 Reports.Read

    private static readonly DateTime SeededOn = new(2026, 8, 22, 0, 0, 0, DateTimeKind.Utc);
    private static readonly Dictionary<long, long[]> RoleClaims = new()
    {
        // SuperAdmin: all currently defined permissions.
        [1] = [1, 2, 3, 4, 5, 6],

        // Admin: operational management, excluding role management.
        [2] = [1, 2, 4, 5, 6],

        // Manager: daily operations and reporting.
        [3] = [1, 4, 5, 6],

        // Employee: limited catalog and order access.
        [4] = [1, 4],

        // User: storefront catalog access only.
        [5] = [4, 5, 6]
    };

    internal static void Add(ModelBuilder modelBuilder)
    {
        DateTime seededOn = SeededOn;

        var seedRows =
            from role in RoleClaims
            from claimId in role.Value
            let sequence = GetSequence(role.Key, claimId)
            select new
            {
                Id = -sequence,
                Uid = Guid.Parse(
                    $"60000000-0000-0000-0000-{sequence:D12}"),
                RoleId = role.Key,
                ClaimTypeId = claimId,
                ClaimValue = "true",
                CreatedAt = seededOn,
                CreatedOn = seededOn,
                CreatedBy = "System",
                UpdatedOn = (DateTime?)null
            };

        modelBuilder.Entity(
            "App.Core.Persistence.Identity.Claims.Entities.RoleClaim")
            .HasData(seedRows);
    }

    private static long GetSequence(long roleId, long claimId)
    {
        // Deterministic, unique sequence for each role-claim pair.
        long roleNumber = Math.Abs(roleId);
        long claimNumber = Math.Abs(claimId);

        return ((roleNumber - 1) * 100) + claimNumber;
    }
}
