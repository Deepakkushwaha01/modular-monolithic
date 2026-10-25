using App.Core.Command.Identity.Claims;
using App.Core.Domain.Identity.Claims;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Commands.Claims;

public sealed class UpdateUserClaimCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_UpdatesExistingUserClaim()
    {
        var user = await SeedUserAsync();
        var claimType = await SeedClaimTypeAsync();
        Assert.That((await UserClaims.InsertAsync(UserClaim.Create(user.Id, claimType.Id, "before"))).IsSuccess, Is.True);
        Assert.That((await UnitOfWork.SaveChangesAsync()).IsSuccess, Is.True);
        var existing = (await UserClaims.GetUserClaimsAsync()).Value.Single();

        var result = await new UpdateUserClaimCommandHandler(UserClaims, UnitOfWork)
            .ExecuteAsync(new(existing.Uid, " after "));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That((await UserClaims.GetUserClaimByUidAsync(existing.Uid)).Value.ClaimValue, Is.EqualTo("after"));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownUserClaim()
    {
        var result = await new UpdateUserClaimCommandHandler(UserClaims, UnitOfWork)
            .ExecuteAsync(new(Guid.NewGuid(), "value"));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }
}
