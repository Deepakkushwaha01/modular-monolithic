using App.API.Settings;
using App.Core.Persistence.Configurations.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace App.API.Registers;

public static partial class Register
{
    public static IServiceCollection RegisterDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        DatabaseConfigurationSettings databaseSettings = new DatabaseConfigurationSettings(configuration);
        // register database context for command
        services.AddDbContext<IAppDbContext, AppDbContext>(options =>
        {
            options.UseNpgsql(databaseSettings.DefaultDBConnection, 
            npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure();
            });
        });

        return services;
    }
}