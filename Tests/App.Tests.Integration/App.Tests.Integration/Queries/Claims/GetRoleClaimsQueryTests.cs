using App.Core.Domain.Identity.Claims;
using App.Core.Queries.Identity.Claims;

namespace App.Tests.Integration.Queries.Claims;

public sealed class GetRoleClaimsQueryTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_ReturnsEmptyListWhenNoClaimsExist()
    {
        var result = await new GetRoleClaimsQueryHandler(RoleClaims).ExecuteAsync(new());

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Empty);
    }

    [Test]
    public async Task ExecuteAsync_ReturnsAllRoleClaims()
    {
        var role = await SeedRoleAsync();
        var claimType = await SeedClaimTypeAsync();
        await SeedRoleClaimAsync(role.Id, claimType.Id, "admin");

        var result = await new GetRoleClaimsQueryHandler(RoleClaims).ExecuteAsync(new());

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Has.Count.EqualTo(1));
        Assert.That(result.Value[0].ClaimValue, Is.EqualTo("admin"));
    }
}
