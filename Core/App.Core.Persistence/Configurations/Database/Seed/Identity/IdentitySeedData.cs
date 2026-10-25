namespace App.Core.Persistence.Configurations.Database.Context.Extensions.Identity.Seeds;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Development seed data for the complete Identity aggregate.
/// Negative primary keys keep the generated database identity sequence available for application data.
/// </summary>
public static class IdentitySeedData
{
    public static void AddIdentitySeedData(this ModelBuilder modelBuilder)
    {
        RoleSeedData.Add(modelBuilder);
        ClaimTypeSeedData.Add(modelBuilder);
        UserSeedData.Add(modelBuilder);
        AddressSeedData.Add(modelBuilder);
        UserRoleSeedData.Add(modelBuilder);
        RoleClaimSeedData.Add(modelBuilder);
        UserClaimSeedData.Add(modelBuilder);
    }
}
