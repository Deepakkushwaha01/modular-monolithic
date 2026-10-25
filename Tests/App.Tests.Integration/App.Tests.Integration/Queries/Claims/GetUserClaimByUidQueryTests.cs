using App.Core.Queries.Identity.Claims;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Queries.Claims;

public sealed class GetUserClaimByUidQueryTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_ReturnsUserClaim()
    {
        var user = await SeedUserAsync();
        var claimType = await SeedClaimTypeAsync();
        await SeedUserClaimAsync(user.Id, claimType.Id, "premium");
        var claim = (await UserClaims.GetUserClaimsAsync()).Value.Single();

        var result = await new GetUserClaimByUidQueryHandler(UserClaims).ExecuteAsync(new(claim.Uid));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.ClaimValue, Is.EqualTo("premium"));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownUid()
    {
        var result = await new GetUserClaimByUidQueryHandler(UserClaims).ExecuteAsync(new(Guid.NewGuid()));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }
}
