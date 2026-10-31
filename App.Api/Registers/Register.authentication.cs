using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using App.Core.Persistence.Identity.Users.Repositories;
using App.Core.SharedLibrary.Enums;
using App.Core.SharedLibrary.Helpers;
using App.Core.SharedLibrary.Patterns.Result;
using App.Core.SharedLibrary.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace App.API.Registers;

public static partial class Register
{
    public static IServiceCollection RegisterAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                JwtSettings jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
                    ?? new JwtSettings();

                options.MapInboundClaims = false;
                options.TokenValidationParameters = JwtTokenHelper.CreateValidationParameters(
                    jwtSettings,
                    Encoding.UTF8.GetBytes(jwtSettings.SigningKey));
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        string? subject = context.Principal?.FindFirstValue(JwtRegisteredClaimNames.Sub);
                        string? portal = context.Principal?.FindFirstValue(JwtTokenHelper.JwtClaimTypes.Portal);
                        if (!Guid.TryParse(subject, out Guid userUid)
                            || !Enum.TryParse(portal, ignoreCase: false, out Portal tokenPortal)
                            || !Enum.IsDefined(tokenPortal))
                        {
                            context.Fail("The access token is invalid.");
                            return;
                        }

                        RedisCacheService redisCache = context.HttpContext.RequestServices
                            .GetRequiredService<RedisCacheService>();
                        bool isBlocked = await redisCache.IsInSetAsync(
                            RedisCacheConstants.BlockedUsersKey,
                            userUid.ToString("D"));
                        if (isBlocked)
                        {
                            context.Fail("The access token is invalid.");
                            return;
                        }
                    }
                };
            });

        return services;
    }
}
