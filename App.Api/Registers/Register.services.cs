using App.Core.Persistence.Authentication.Users.Repositories;
using App.Core.Persistence.Orders.Repositories;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;

namespace App.API.Registers;

public static partial class Register
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment _)
    {
        #region Repositories

        // services.AddScoped<IUserRepository, UserRepository>();
        // services.AddScoped<IRolesRepository, RolesRepository>();
        // services.AddScoped<IClaimsRepository, ClaimsRepository>();

        #endregion

        services.AddSharedMediatrServices();

        return services;
    }
}