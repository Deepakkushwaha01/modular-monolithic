using App.Core.Command.Identity.Claims;
using App.Core.Domain.Identity.Claims;
using App.Core.Domain.Identity.Roles;

namespace App.Tests.Integration.Commands.Claims;

public sealed class CreateRoleClaimCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_CreatesRoleClaim()
    {
        (Role Entity, long Id) role = await SeedRoleAsync();
        (ClaimType Entity, long Id) claimType = await SeedClaimTypeAsync();
        CreateRoleClaimCommandHandler handler = new(RoleClaims, UnitOfWork);

        Result<Guid> result = await handler.ExecuteAsync(new(role.Entity.Uid, claimType.Entity.Uid, " enabled "));

        Assert.That(result.IsSuccess, Is.True);
        Result<RoleClaim> saved = await RoleClaims.GetRoleClaimByUidAsync(result.Value);
        Assert.That(saved.IsSuccess, Is.True);
        Assert.That(saved.Value.ClaimValue, Is.EqualTo("enabled"));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsFailureWhenForeignKeyIsInvalid()
    {
        (ClaimType Entity, long Id) claimType = await SeedClaimTypeAsync();

        Result<Guid> result = await new CreateRoleClaimCommandHandler(RoleClaims, UnitOfWork)
            .ExecuteAsync(new(Guid.NewGuid(), claimType.Entity.Uid, "enabled"));

        Assert.That(result.IsFailure, Is.True);
    }
}
