using System.IdentityModel.Tokens.Jwt;
using App.API.Controllers.Identity.Contracts;
using App.Core.Command.Identity.Authentication;
using App.Core.SharedLibrary.Patterns.Result;
using App.Core.SharedLibrary.Authentication;
using App.Core.SharedLibrary.Helpers;
using App.Core.SharedLibrary.Settings;
using App.Core.Domain.Identity.Roles;
using App.Core.Domain.Identity.Users;
using App.Core.Persistence.Identity.Users.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using App.Core.Domain.Identity.Claims;

namespace App.Tests.Integration.Authentication;

public sealed class LoginCommandTests : IntegrationTestBase
{
    private const string Password = "StrongPass123";
    private const string SigningKey = "test-signing-key-at-least-32-characters-long";

    [Test]
    public async Task LoginCommand_ReturnsJwtWithPortalRolesAndClaims()
    {
        (Role Entity, long Id) role = await SeedRoleAsync(slug: "store-admin");
        IPasswordHasher<User> passwordHasher = new PasswordHasher<User>();
        User passwordContext = User.Create("login@example.com", string.Empty, "+14155552671");
        string passwordHash = passwordHasher.HashPassword(passwordContext, Password);
        User user = User.Create("login@example.com", passwordHash, "+14155552671");
        Assert.That((await Users.InsertAsync(user)).IsSuccess, Is.True);
        Assert.That((await UnitOfWork.SaveChangesAsync()).IsSuccess, Is.True);

        Assert.That(
            (await UserRoles.InsertAsync(UserRole.Create(user.Uid, role.Entity.Uid))).IsSuccess,
            Is.True);
        Assert.That((await UnitOfWork.SaveChangesAsync()).IsSuccess, Is.True);
        (ClaimType Entity, long Id) permission = await SeedClaimTypeAsync(name: "catalog.read", module: "storefront");
        await SeedRoleClaimAsync(role.Entity.Uid, permission.Entity.Uid);

        LoginCommandHandler handler = new(Users, UnitOfWork, passwordHasher);
        Result<AuthenticatedUser> authResult = await handler.ExecuteAsync(
            new LoginCommand(" LOGIN@example.com ", Password), CancellationToken.None);
        Assert.That(authResult.IsSuccess, Is.True);

        JwtTokenHelper tokenHelper = new(Options.Create(new JwtSettings
        {
            Issuer = "test-issuer",
            Audience = "test-audience",
            SigningKey = SigningKey,
            Portal = "storefront",
            AccessTokenLifetimeMinutes = 15
        }));
        (string accessToken, DateTimeOffset expiresAt) = tokenHelper.CreateToken(authResult.Value);
        Assert.That(authResult.Value.Uid, Is.EqualTo(user.Uid));
        Assert.That(authResult.Value.Roles, Is.EqualTo(new[] { "store-admin" }));
        Assert.That(expiresAt, Is.GreaterThan(DateTimeOffset.UtcNow));
        Assert.That(tokenHelper.TryValidateToken(accessToken, out _), Is.True);

        JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
        Assert.That(token.Claims.Single(claim => claim.Type == "portal").Value, Is.EqualTo("storefront"));
        Assert.That(token.Claims.Where(claim => claim.Type == "role").Select(claim => claim.Value),
            Is.EqualTo(new[] { "store-admin" }));
        Assert.That(token.Claims.Single(claim => claim.Type == "catalog.read").Value, Is.EqualTo("true"));
        Assert.That(token.Claims.Single(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value,
            Is.EqualTo(user.Uid.ToString()));
    }

    [Test]
    public async Task LoginCommand_RejectsIncorrectPassword()
    {
        IPasswordHasher<User> passwordHasher = new PasswordHasher<User>();
        User passwordContext = User.Create("login@example.com", string.Empty, "+14155552671");
        string passwordHash = passwordHasher.HashPassword(passwordContext, Password);
        User user = User.Create("login@example.com", passwordHash, "+14155552671");
        Assert.That((await Users.InsertAsync(user)).IsSuccess, Is.True);
        Assert.That((await UnitOfWork.SaveChangesAsync()).IsSuccess, Is.True);

        LoginCommandHandler handler = new(Users, UnitOfWork, passwordHasher);
        Result<AuthenticatedUser> result = await handler.ExecuteAsync(
            new LoginCommand(user.Email, "WrongPass123"), CancellationToken.None);

        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.HttpStatusCode, Is.EqualTo(System.Net.HttpStatusCode.Unauthorized));
    }
}
