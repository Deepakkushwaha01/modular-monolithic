using App.Core.Queries.Identity.Claims;

namespace App.Tests.Integration.Queries.Claims;

public sealed class GetUserClaimsQueryTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_ReturnsEmptyListWhenNoClaimsExist()
    {
        var result = await new GetUserClaimsQueryHandler(UserClaims).ExecuteAsync(new());

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Empty);
    }

    [Test]
    public async Task ExecuteAsync_ReturnsAllUserClaims()
    {
        var user = await SeedUserAsync();
        var claimType = await SeedClaimTypeAsync();
        await SeedUserClaimAsync(user.Id, claimType.Id, "premium");

        var result = await new GetUserClaimsQueryHandler(UserClaims).ExecuteAsync(new());

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Has.Count.EqualTo(1));
        Assert.That(result.Value[0].ClaimValue, Is.EqualTo("premium"));
    }
}
