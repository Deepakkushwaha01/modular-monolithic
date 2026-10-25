using App.Core.Command.Identity.Claims;
using App.Core.Domain.Identity.Claims;

namespace App.Tests.Integration.Commands.Claims;

public sealed class CreateRoleClaimCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_CreatesRoleClaim()
    {
        var role = await SeedRoleAsync();
        var claimType = await SeedClaimTypeAsync();
        CreateRoleClaimCommandHandler handler = new(RoleClaims, UnitOfWork);

        var result = await handler.ExecuteAsync(new(role.Id, claimType.Id, " enabled "));

        Assert.That(result.IsSuccess, Is.True);
        var saved = await RoleClaims.GetRoleClaimByUidAsync(result.Value);
        Assert.That(saved.IsSuccess, Is.True);
        Assert.That(saved.Value.ClaimValue, Is.EqualTo("enabled"));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsFailureWhenForeignKeyIsInvalid()
    {
        var claimType = await SeedClaimTypeAsync();

        var result = await new CreateRoleClaimCommandHandler(RoleClaims, UnitOfWork)
            .ExecuteAsync(new(long.MaxValue, claimType.Id, "enabled"));

        Assert.That(result.IsFailure, Is.True);
    }
}
