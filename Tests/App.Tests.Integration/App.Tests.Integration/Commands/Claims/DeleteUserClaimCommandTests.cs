using App.Core.Command.Identity.Claims;
using App.Core.Domain.Identity.Claims;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Commands.Claims;

public sealed class DeleteUserClaimCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_DeletesExistingUserClaim()
    {
        var user = await SeedUserAsync();
        var claimType = await SeedClaimTypeAsync();
        Assert.That((await UserClaims.InsertAsync(UserClaim.Create(user.Id, claimType.Id, "true"))).IsSuccess, Is.True);
        Assert.That((await UnitOfWork.SaveChangesAsync()).IsSuccess, Is.True);
        var claim = (await UserClaims.GetUserClaimsAsync()).Value.Single();

        var result = await new DeleteUserClaimCommandHandler(UserClaims, UnitOfWork)
            .ExecuteAsync(new(claim.Uid));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That((await UserClaims.GetUserClaimByUidAsync(claim.Uid)).ResultType, Is.EqualTo(ResultType.NotFound));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownUserClaim()
    {
        var result = await new DeleteUserClaimCommandHandler(UserClaims, UnitOfWork)
            .ExecuteAsync(new(Guid.NewGuid()));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }
}
