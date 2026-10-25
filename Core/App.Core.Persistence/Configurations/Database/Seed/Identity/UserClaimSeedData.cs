namespace App.Core.Persistence.Configurations.Database.Context.Extensions.Identity.Seeds;

using Microsoft.EntityFrameworkCore;

internal static class UserClaimSeedData
{
    private static readonly DateTime SeededOn =
        new(2026, 8, 22, 0, 0, 0, DateTimeKind.Utc);

    internal static void Add(ModelBuilder modelBuilder)
    {
        // NOTE (Hinglish):
        // UserClaim ka use tab hota hai jab kisi specific user ko
        // uske role se alag koi additional permission ya restriction deni ho.
        //
        // Example:
        // - Agar User role ke paas sirf Orders.Read permission hai,
        //   lekin kisi particular user ko Orders.Edit bhi dena hai,
        //   to us user ke liye UserClaim add kar sakte hain.
        //
        // - Agar kisi specific user ki permission ko restrict karna hai,
        //   to ClaimValue = "false" use kar sakte hain, lekin authorization
        //   logic mein false ki precedence explicitly handle honi chahiye.
        //
        // Important:
        // Har user ke liye UserClaim add karna zaroori nahi hai.
        // Common permissions RoleClaims se milengi:
        // UserRoles -> RoleClaims -> ClaimTypes.
        //
        // UserClaim sirf user-specific exceptions ke liye use karein.
        // Agar RoleClaim aur UserClaim conflict karte hain, to authorization
        // logic mein decide karein ki allow hoga ya deny.
        //
        // Seed data sirf initial/static records ke liye rakhein.
        // Real user-specific claims normally application logic se manage honge.

        modelBuilder.Entity(
            "App.Core.Persistence.Identity.Claims.Entities.UserClaim")
            .HasData(
                new
                {
                    Id = 1L,
                    Uid = Guid.Parse(
                        "70000000-0000-0000-0000-000000000001"),
                    UserId = 2L,
                    ClaimTypeId = 3L,
                    ClaimValue = "false",
                    CreatedOn = SeededOn,
                    CreatedBy = "System",
                    UpdatedOn = (DateTime?)null
                },
                new
                {
                    Id = 2L,
                    Uid = Guid.Parse(
                        "70000000-0000-0000-0000-000000000002"),
                    UserId = 3L,
                    ClaimTypeId = 2L,
                    ClaimValue = "true",
                    CreatedOn = SeededOn,
                    CreatedBy = "System",
                    UpdatedOn = (DateTime?)null
                });
    }
}