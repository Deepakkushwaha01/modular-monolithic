using App.Core.Command.Identity.Claims;
using App.Core.Domain.Identity.Claims;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Commands.Claims;

public sealed class UpdateRoleClaimCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_UpdatesExistingRoleClaim()
    {
        var role = await SeedRoleAsync();
        var claim = await SeedClaimTypeAsync();
        var inserted = await RoleClaims.InsertAsync(RoleClaim.Create(role.Id, claim.Id, "before"));
        Assert.That(inserted.IsSuccess, Is.True);
        Assert.That((await UnitOfWork.SaveChangesAsync()).IsSuccess, Is.True);
        var existing = (await RoleClaims.GetRoleClaimsAsync()).Value.Single();

        var result = await new UpdateRoleClaimCommandHandler(RoleClaims, UnitOfWork)
            .ExecuteAsync(new(existing.Uid, " after "));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That((await RoleClaims.GetRoleClaimByUidAsync(existing.Uid)).Value.ClaimValue, Is.EqualTo("after"));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownRoleClaim()
    {
        var result = await new UpdateRoleClaimCommandHandler(RoleClaims, UnitOfWork)
            .ExecuteAsync(new(Guid.NewGuid(), "value"));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }
}
