using App.Core.Command.Identity.Claims;
using App.Core.Domain.Identity.Claims;
using App.Core.Domain.Identity.Users;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Commands.Claims;

public sealed class DeleteUserClaimCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_DeletesExistingUserClaim()
    {
        (User Entity, long Id) user = await SeedUserAsync();
        (ClaimType Entity, long Id) claimType = await SeedClaimTypeAsync();
        Assert.That((await UserClaims.InsertAsync(UserClaim.Create(user.Entity.Uid, claimType.Entity.Uid, "true"))).IsSuccess, Is.True);
        Assert.That((await UnitOfWork.SaveChangesAsync()).IsSuccess, Is.True);
        UserClaim claim = (await UserClaims.GetUserClaimsAsync()).Value.Single();

        Result result = await new DeleteUserClaimCommandHandler(UserClaims, UnitOfWork)
            .ExecuteAsync(new(claim.Uid));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That((await UserClaims.GetUserClaimByUidAsync(claim.Uid)).ResultType, Is.EqualTo(ResultType.NotFound));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownUserClaim()
    {
        Result result = await new DeleteUserClaimCommandHandler(UserClaims, UnitOfWork)
            .ExecuteAsync(new(Guid.NewGuid()));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }
}
