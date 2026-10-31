using App.Core.Domain.Identity.Claims;
using App.Core.Domain.Identity.Users;
using App.Core.Queries.Identity.Claims;

namespace App.Tests.Integration.Queries.Claims;

public sealed class GetUserClaimsQueryTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_ReturnsEmptyListWhenNoClaimsExist()
    {
        Result<List<UserClaim>> result = await new GetUserClaimsQueryHandler(UserClaims).ExecuteAsync(new());

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Empty);
    }

    [Test]
    public async Task ExecuteAsync_ReturnsAllUserClaims()
    {
        (User Entity, long Id) user = await SeedUserAsync();
        (ClaimType Entity, long Id) claimType = await SeedClaimTypeAsync();
        await SeedUserClaimAsync(user.Entity.Uid, claimType.Entity.Uid, "premium");

        Result<List<UserClaim>> result = await new GetUserClaimsQueryHandler(UserClaims).ExecuteAsync(new());

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Has.Count.EqualTo(1));
        Assert.That(result.Value[0].ClaimValue, Is.EqualTo("premium"));
    }
}
