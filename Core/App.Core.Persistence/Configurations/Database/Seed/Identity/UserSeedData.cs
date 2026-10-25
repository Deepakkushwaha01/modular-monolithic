
namespace App.Core.Persistence.Configurations.Database.Context.Extensions.Identity.Seeds;

using Microsoft.EntityFrameworkCore;

internal static class UserSeedData
{
    private static readonly DateTime SeededOn =
        new(2026, 8, 22, 0, 0, 0, DateTimeKind.Utc);

    // Generate this hash once using the same password hasher
    // configured by the application, then keep it constant.
    private const string PasswordHash =
        "AQAAAAIAAYagAAAAEKTBMBC8DMLypEU3loqh5G1gGqrhOMn8/AUPEs44d1jqU9/S9AmoEVOAGKcvflxGZQ==";
    // 404@BadDeveloper --

    internal static void Add(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.Entity(
            "App.Core.Persistence.Identity.Users.Entities.User")
            .HasData(new
            {
                Id = 1L,
                Uid = Guid.Parse(
                    "30000000-0000-0000-0000-000000000001"),
                Email = "dk7495570@gmail.com",
                Password = PasswordHash,
                FirstName = "Deepak",
                LastName = "Kushwaha",
                Phone = "+919582083811",
                IsPhoneVerified = false,
                IsActive = true,
                IsBlocked = false,
                IsEmailVerified = true,
                LastLoginDate = (DateTime?)null,
                ProfilePhoto = (string?)null,
                CreatedOn = SeededOn,
                CreatedBy = "System",
                UpdatedBy = (string?)null,
                UpdatedOn = (DateTime?)null
            },
            new
            {
                Id = 2L,
                Uid = Guid.Parse(
                    "30000000-0000-0000-0000-000000000002"),
                Email = "dk7495570+admin@gmail.com",
                Password = PasswordHash,
                FirstName = "Deepak",
                LastName = "Kushwaha (Admin)",
                Phone = "+919582083811",
                IsPhoneVerified = false,
                IsActive = true,
                IsBlocked = false,
                IsEmailVerified = true,
                LastLoginDate = (DateTime?)null,
                ProfilePhoto = (string?)null,
                CreatedOn = SeededOn,
                CreatedBy = "System",
                UpdatedBy = (string?)null,
                UpdatedOn = (DateTime?)null
            },
            new
            {
                Id = 3L,
                Uid = Guid.Parse(
                    "30000000-0000-0000-0000-000000000003"),
                Email = "dk7495570+manager@gmail.com",
                Password = PasswordHash,
                FirstName = "Deepak",
                LastName = "Kushwaha (Manager)",
                Phone = "+919582083811",
                IsPhoneVerified = false,
                IsActive = true,
                IsBlocked = false,
                IsEmailVerified = true,
                LastLoginDate = (DateTime?)null,
                ProfilePhoto = (string?)null,
                CreatedOn = SeededOn,
                CreatedBy = "System",
                UpdatedBy = (string?)null,
                UpdatedOn = (DateTime?)null
            });
    }
}
