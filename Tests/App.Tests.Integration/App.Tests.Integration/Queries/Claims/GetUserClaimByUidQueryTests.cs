using App.Core.Domain.Identity.Claims;
using App.Core.Domain.Identity.Users;
using App.Core.Queries.Identity.Claims;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Queries.Claims;

public sealed class GetUserClaimByUidQueryTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_ReturnsUserClaim()
    {
        (User Entity, long Id) user = await SeedUserAsync();
        (ClaimType Entity, long Id) claimType = await SeedClaimTypeAsync();
        await SeedUserClaimAsync(user.Entity.Uid, claimType.Entity.Uid, "premium");
        UserClaim claim = (await UserClaims.GetUserClaimsAsync()).Value.Single();

        Result<UserClaim> result = await new GetUserClaimByUidQueryHandler(UserClaims).ExecuteAsync(new(claim.Uid));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.ClaimValue, Is.EqualTo("premium"));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownUid()
    {
        Result<UserClaim> result = await new GetUserClaimByUidQueryHandler(UserClaims).ExecuteAsync(new(Guid.NewGuid()));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }
}
