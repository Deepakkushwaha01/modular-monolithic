using System.Text;
using App.Core.SharedLibrary.Helpers;
using App.Core.SharedLibrary.Settings;

namespace App.API.Registers;

public static partial class Register
{
    public static IServiceCollection RegisterJwtServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<JwtSettings>()
            .Bind(configuration.GetSection(JwtSettings.SectionName))
            .Validate(settings => !string.IsNullOrWhiteSpace(settings.Issuer), "Jwt:Issuer is required.")
            .Validate(settings => !string.IsNullOrWhiteSpace(settings.Audience), "Jwt:Audience is required.")
            .Validate(settings => settings.AccessTokenLifetimeMinutes is > 0 and <= 1440,
                "Jwt:AccessTokenLifetimeMinutes must be between 1 and 1440.")
            .Validate(settings => settings.RefreshTokenLifetimeDays is > 0 and <= 365,
                "Jwt:RefreshTokenLifetimeDays must be between 1 and 365.")
            .Validate(settings => !string.IsNullOrWhiteSpace(settings.SigningKey)
                && Encoding.UTF8.GetByteCount(settings.SigningKey) >= 32,
                "Jwt:SigningKey must contain at least 32 UTF-8 bytes. Configure it in API settings or with Jwt__SigningKey.")
            .ValidateOnStart();

        services.AddSingleton<JwtTokenHelper>();
        return services;
    }
}
