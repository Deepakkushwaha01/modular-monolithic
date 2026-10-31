using App.Core.Command.Identity.Claims;
using App.Core.Domain.Identity.Claims;
using App.Core.Domain.Identity.Roles;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Commands.Claims;

public sealed class DeleteRoleClaimCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_DeletesExistingRoleClaim()
    {
        (Role Entity, long Id) role = await SeedRoleAsync();
        (ClaimType Entity, long Id) claimType = await SeedClaimTypeAsync();
        Assert.That((await RoleClaims.InsertAsync(RoleClaim.Create(role.Entity.Uid, claimType.Entity.Uid, "true"))).IsSuccess, Is.True);
        Assert.That((await UnitOfWork.SaveChangesAsync()).IsSuccess, Is.True);
        RoleClaim claim = (await RoleClaims.GetRoleClaimsAsync()).Value.Single();

        Result result = await new DeleteRoleClaimCommandHandler(RoleClaims, UnitOfWork)
            .ExecuteAsync(new(claim.Uid));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That((await RoleClaims.GetRoleClaimByUidAsync(claim.Uid)).ResultType, Is.EqualTo(ResultType.NotFound));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownRoleClaim()
    {
        Result result = await new DeleteRoleClaimCommandHandler(RoleClaims, UnitOfWork)
            .ExecuteAsync(new(Guid.NewGuid()));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }
}
