
namespace App.Core.Persistence.Configurations.Database.Context.Extensions.Identity.Seeds;

using Microsoft.EntityFrameworkCore;

internal static class AddressSeedData
{
    private static readonly DateTime SeededOn = new(2026, 8, 22, 0, 0, 0, DateTimeKind.Utc);
    internal static void Add(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity(
            "App.Core.Persistence.Identity.Addresses.Entities.Address")
            .HasData(new
            {
                Id = 1L,
                Uid = Guid.Parse("40000000-0000-0000-0000-000000000001"),
                UserId = 1L,
                AddressType = "Billing",
                AddressLine1 = "1 Seed Data Way",
                AddressLine2 = (string?)null,
                City = "Mumbai",
                State = "Maharashtra",
                ZipCode = "400001",
                Country = "India",
                IsDefault = true,
                CreatedOn = SeededOn,
                CreatedBy = "System",
                UpdatedOn = (DateTime?)null
            });
    }
}
