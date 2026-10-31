using App.Core.Domain.Identity.Users;
using App.Core.Persistence.Identity.Addresses.Repositories;
using App.Core.Persistence.Identity.Claims.Repositories;
using App.Core.Persistence.Identity.Roles.Repositories;
using App.Core.Persistence.Identity.Users.Repositories;
using App.Core.Persistence.Identity.Authentication.Repositories;
using Microsoft.AspNetCore.Identity;
using App.Core.Persistence.Configurations.Database.Context.UnitOfWork;

namespace App.API.Registers;

public static partial class Register
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        #region Database Context
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        #endregion

        #region Identities Repositories

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IClaimTypeRepository, ClaimTypeRepository>();
        services.AddScoped<IRoleClaimRepository, RoleClaimRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserClaimRepository, UserClaimRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();

        #endregion

        services.AddSharedMediatrServices();
        return services;
    }
}
