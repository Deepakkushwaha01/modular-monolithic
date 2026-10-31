
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using App.Core.SharedLibrary.Enums;
using App.Core.SharedLibrary.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace App.Core.SharedLibrary.Helpers;

public sealed class JwtTokenHelper(IOptions<JwtSettings> options)
{
    private readonly JwtSettings _settings = options.Value;

    private readonly byte[] _signingKey =
        Encoding.UTF8.GetBytes(options.Value.SigningKey);

    public static class JwtClaimTypes
    {
        // Standard JWT claims
        public const string Subject = JwtRegisteredClaimNames.Sub;
        public const string Email = JwtRegisteredClaimNames.Email;
        public const string JwtId = JwtRegisteredClaimNames.Jti;
        public const string Issuer = JwtRegisteredClaimNames.Iss;
        public const string Audience = JwtRegisteredClaimNames.Aud;
        public const string Expiration = JwtRegisteredClaimNames.Exp;
        public const string NotBefore = JwtRegisteredClaimNames.Nbf;
        public const string IssuedAt = JwtRegisteredClaimNames.Iat;

        // Application-specific claims
        public const string Role = "role";
        public const string Permission = "permission";
        public const string Portal = "portal";
    }

    // Access token creation
    public string CreateAccessToken(
        Guid uid,
        string email,
        Portal portal,
        IEnumerable<string> roles,
        IEnumerable<string> permissions)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentNullException.ThrowIfNull(roles);
        ArgumentNullException.ThrowIfNull(permissions);

        DateTimeOffset issuedAt = DateTimeOffset.UtcNow;
        DateTimeOffset expiresAt = issuedAt.AddMinutes(
            _settings.AccessTokenLifetimeMinutes);

        List<Claim> tokenClaims =
        [
            new(JwtClaimTypes.Subject, uid.ToString()),
            new(JwtClaimTypes.Email, email),
            new(JwtClaimTypes.JwtId, Guid.NewGuid().ToString("N")),
            new(JwtClaimTypes.Portal, portal.ToString())
        ];

        tokenClaims.AddRange(
            roles
                .Where(role => !string.IsNullOrWhiteSpace(role))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Select(role => new Claim(JwtClaimTypes.Role, role)));

        tokenClaims.AddRange(
            permissions
                .Where(permission =>
                    !string.IsNullOrWhiteSpace(permission))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Select(permission =>
                    new Claim(JwtClaimTypes.Permission, permission)));

        JwtSecurityToken token = new(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: tokenClaims,
            notBefore: issuedAt.UtcDateTime,
            expires: expiresAt.UtcDateTime,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(_signingKey),
                SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // Access token validation
    public bool TryValidateAccessToken(
        string accessToken,
        out ClaimsPrincipal? principal)
    {
        return TryValidateToken(accessToken, out principal);
    }

    public bool TryValidateToken(
        string token,
        out ClaimsPrincipal? principal)
    {
        principal = null;

        if (string.IsNullOrWhiteSpace(token))
            return false;

        try
        {
            principal = new JwtSecurityTokenHandler().ValidateToken(
                token,
                CreateValidationParameters(_settings, _signingKey),
                out _);

            return true;
        }
        catch (Exception exception) when (
            exception is SecurityTokenException or ArgumentException)
        {
            principal = null;
            return false;
        }
    }

    // Refresh token creation
    public RefreshTokenResult CreateRefreshToken()
    {
        if (_settings.RefreshTokenLifetimeDays <= 0)
        {
            throw new InvalidOperationException(
                "Refresh token lifetime must be greater than zero.");
        }

        DateTimeOffset expiresAt = DateTimeOffset.UtcNow.AddDays(
            _settings.RefreshTokenLifetimeDays);

        byte[] randomBytes = RandomNumberGenerator.GetBytes(64);

        string refreshToken = Base64UrlEncoder.Encode(randomBytes);
        string tokenHash = HashRefreshToken(refreshToken);

        return new RefreshTokenResult(
            refreshToken,
            tokenHash,
            expiresAt);
    }

    public DateTimeOffset GetAccessTokenExpiresAt() =>
        DateTimeOffset.UtcNow.AddMinutes(_settings.AccessTokenLifetimeMinutes);

    public string GetRefreshTokenHash(string refreshToken) => HashRefreshToken(refreshToken);

    // Refresh token validation
    public bool TryValidateRefreshToken(
        string refreshToken,
        string storedTokenHash,
        DateTimeOffset expiresAt,
        bool isRevoked)
    {
        if (string.IsNullOrWhiteSpace(refreshToken) ||
            string.IsNullOrWhiteSpace(storedTokenHash))
        {
            return false;
        }

        if (isRevoked || expiresAt <= DateTimeOffset.UtcNow)
            return false;

        string computedHash = HashRefreshToken(refreshToken);

        byte[] computedBytes = Encoding.UTF8.GetBytes(computedHash);
        byte[] storedBytes = Encoding.UTF8.GetBytes(storedTokenHash);

        return computedBytes.Length == storedBytes.Length &&
            CryptographicOperations.FixedTimeEquals(
                computedBytes,
                storedBytes);
    }

    // Refresh tokens are stored as hashes, never as plaintext.
    private static string HashRefreshToken(string refreshToken)
    {
        byte[] hash = SHA256.HashData(
            Encoding.UTF8.GetBytes(refreshToken));

        return Convert.ToHexString(hash);
    }

    // Shared JWT validation configuration
    public static TokenValidationParameters CreateValidationParameters(
        JwtSettings jwtSettings,
        byte[] signingKey) => new()
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,

        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(signingKey),

        RequireSignedTokens = true,
        RequireExpirationTime = true,
        ValidateLifetime = true,

        ClockSkew = TimeSpan.FromSeconds(30),

        ValidAlgorithms = [SecurityAlgorithms.HmacSha256],

        NameClaimType = JwtClaimTypes.Email,
        RoleClaimType = JwtClaimTypes.Role
    };
}

// Refresh token generation result
public sealed record RefreshTokenResult(
    string Token,
    string TokenHash,
    DateTimeOffset ExpiresAt);
