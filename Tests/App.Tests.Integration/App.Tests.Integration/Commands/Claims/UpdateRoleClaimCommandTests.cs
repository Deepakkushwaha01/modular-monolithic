using App.Core.Command.Identity.Claims;
using App.Core.Domain.Identity.Claims;
using App.Core.Domain.Identity.Roles;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Commands.Claims;

public sealed class UpdateRoleClaimCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_UpdatesExistingRoleClaim()
    {
        (Role Entity, long Id) role = await SeedRoleAsync();
        (ClaimType Entity, long Id) claim = await SeedClaimTypeAsync();
        Result inserted = await RoleClaims.InsertAsync(RoleClaim.Create(role.Entity.Uid, claim.Entity.Uid, "before"));
        Assert.That(inserted.IsSuccess, Is.True);
        Assert.That((await UnitOfWork.SaveChangesAsync()).IsSuccess, Is.True);
        RoleClaim existing = (await RoleClaims.GetRoleClaimsAsync()).Value.Single();

        Result<Guid> result = await new UpdateRoleClaimCommandHandler(RoleClaims, UnitOfWork)
            .ExecuteAsync(new(existing.Uid, " after "));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That((await RoleClaims.GetRoleClaimByUidAsync(existing.Uid)).Value.ClaimValue, Is.EqualTo("after"));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownRoleClaim()
    {
        Result<Guid> result = await new UpdateRoleClaimCommandHandler(RoleClaims, UnitOfWork)
            .ExecuteAsync(new(Guid.NewGuid(), "value"));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }
}
