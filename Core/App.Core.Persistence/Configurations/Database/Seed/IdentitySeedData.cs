namespace App.Core.Persistence.Configurations.Database.Context.Extensions.Identity;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Development seed data for the complete Identity aggregate.
/// Negative primary keys keep the generated database identity sequence available for application data.
/// </summary>
public static class IdentitySeedData
{
    private static readonly DateTime SeededOn = new(2026, 8, 22, 0, 0, 0, DateTimeKind.Utc);

    public static void AddIdentitySeedData(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity("App.Core.Persistence.Identity.Roles.Entities.Role").HasData(
            new { Id = -1L, Uid = Guid.Parse("10000000-0000-0000-0000-000000000001"), Name = "Administrator", Slug = "administrator", Description = "System administrator", IsSystemRole = true, IsActive = true, CreatedOn = SeededOn, UpdatedOn = (DateTime?)null },
            new { Id = -2L, Uid = Guid.Parse("10000000-0000-0000-0000-000000000002"), Name = "Customer", Slug = "customer", Description = "Default customer role", IsSystemRole = true, IsActive = true, CreatedOn = SeededOn, UpdatedOn = (DateTime?)null });

        modelBuilder.Entity("App.Core.Persistence.Identity.Claims.Entities.ClaimType").HasData(
            new { Id = -1L, Uid = Guid.Parse("20000000-0000-0000-0000-000000000001"), Name = "Identity.Users.Manage", Description = "Manage identity users", DataType = App.Core.Persistence.Identity.Claims.Entities.ClaimDataType.Boolean, Module = "Identity", IsActive = true, CreatedOn = SeededOn, UpdatedOn = (DateTime?)null },
            new { Id = -2L, Uid = Guid.Parse("20000000-0000-0000-0000-000000000002"), Name = "Identity.Roles.Manage", Description = "Manage identity roles", DataType = App.Core.Persistence.Identity.Claims.Entities.ClaimDataType.Boolean, Module = "Identity", IsActive = true, CreatedOn = SeededOn, UpdatedOn = (DateTime?)null });

        modelBuilder.Entity("App.Core.Persistence.Identity.Users.Entities.User").HasData(
            new { Id = -1L, Uid = Guid.Parse("30000000-0000-0000-0000-000000000001"), Email = "admin@example.com", Password = "$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy", FirstName = "System", LastName = "Administrator", Phone = "+919876543210", IsPhoneVerified = false, IsActive = true, IsBlocked = false, IsEmailVerified = true, LastLoginDate = (DateTime?)null, ProfilePhoto = (string?)null, CreatedOn = SeededOn, UpdatedOn = (DateTime?)null });

        modelBuilder.Entity("App.Core.Persistence.Identity.Addresses.Entities.Address").HasData(
            new { Id = -1L, Uid = Guid.Parse("40000000-0000-0000-0000-000000000001"), UserId = -1L, AddressType = "Billing", AddressLine1 = "1 Seed Data Way", AddressLine2 = (string?)null, City = "Mumbai", State = "Maharashtra", ZipCode = "400001", Country = "India", IsDefault = true, CreatedOn = SeededOn, UpdatedOn = (DateTime?)null });

        modelBuilder.Entity("App.Core.Persistence.Identity.Roles.Entities.UserRole").HasData(
            new { Id = -1L, Uid = Guid.Parse("50000000-0000-0000-0000-000000000001"), UserId = -1L, RoleId = -1L, AssignedAt = SeededOn, AssignedBy = (long?)null, CreatedOn = SeededOn, UpdatedOn = (DateTime?)null });

        modelBuilder.Entity("App.Core.Persistence.Identity.Claims.Entities.RoleClaim").HasData(
            new { Id = -1L, Uid = Guid.Parse("60000000-0000-0000-0000-000000000001"), RoleId = -1L, ClaimTypeId = -1L, ClaimValue = "true", CreatedAt = SeededOn, CreatedOn = SeededOn, UpdatedOn = (DateTime?)null },
            new { Id = -2L, Uid = Guid.Parse("60000000-0000-0000-0000-000000000002"), RoleId = -1L, ClaimTypeId = -2L, ClaimValue = "true", CreatedAt = SeededOn, CreatedOn = SeededOn, UpdatedOn = (DateTime?)null });

        modelBuilder.Entity("App.Core.Persistence.Identity.Claims.Entities.UserClaim").HasData(
            new { Id = -1L, Uid = Guid.Parse("70000000-0000-0000-0000-000000000001"), UserId = -1L, ClaimTypeId = -1L, ClaimValue = "true", CreatedOn = SeededOn, UpdatedOn = (DateTime?)null },
            new { Id = -2L, Uid = Guid.Parse("70000000-0000-0000-0000-000000000002"), UserId = -1L, ClaimTypeId = -2L, ClaimValue = "true", CreatedOn = SeededOn, UpdatedOn = (DateTime?)null });
    }
}
