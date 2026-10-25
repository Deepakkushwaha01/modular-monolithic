using App.Core.Command.Identity.Claims;
using App.Core.Domain.Identity.Claims;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Commands.Claims;

public sealed class DeleteRoleClaimCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_DeletesExistingRoleClaim()
    {
        var role = await SeedRoleAsync();
        var claimType = await SeedClaimTypeAsync();
        Assert.That((await RoleClaims.InsertAsync(RoleClaim.Create(role.Id, claimType.Id, "true"))).IsSuccess, Is.True);
        Assert.That((await UnitOfWork.SaveChangesAsync()).IsSuccess, Is.True);
        var claim = (await RoleClaims.GetRoleClaimsAsync()).Value.Single();

        var result = await new DeleteRoleClaimCommandHandler(RoleClaims, UnitOfWork)
            .ExecuteAsync(new(claim.Uid));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That((await RoleClaims.GetRoleClaimByUidAsync(claim.Uid)).ResultType, Is.EqualTo(ResultType.NotFound));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownRoleClaim()
    {
        var result = await new DeleteRoleClaimCommandHandler(RoleClaims, UnitOfWork)
            .ExecuteAsync(new(Guid.NewGuid()));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }
}
