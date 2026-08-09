namespace App.Core.Persistence.Configurations.Database.Context.Extensions.Identity
{
    using App.Core.Persistence.Configurations.Database.ModelConfigurations.Identity;
    using Microsoft.EntityFrameworkCore;

    public static class IdentityConfigurations
    {
        public static void ConfigureIdentity(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserModelConfiguration());

        }
    }
}