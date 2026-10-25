using App.Core.Queries.Identity.Claims;

namespace App.Tests.Integration.Queries.Claims;

public sealed class GetClaimTypesQueryTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_ReturnsEmptyListWhenNoClaimTypesExist()
    {
        var result = await new GetClaimTypesQueryHandler(ClaimTypes).ExecuteAsync(new());

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Empty);
    }

    [Test]
    public async Task ExecuteAsync_IncludesRequestedRelatedClaims()
    {
        var claimType = await SeedClaimTypeAsync();
        var role = await SeedRoleAsync();
        var user = await SeedUserAsync();
        await SeedRoleClaimAsync(role.Id, claimType.Id);
        await SeedUserClaimAsync(user.Id, claimType.Id);

        var result = await new GetClaimTypesQueryHandler(ClaimTypes)
            .ExecuteAsync(new(IncludeRoleClaims: true, IncludeUserClaims: true));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Has.Count.EqualTo(1));
        Assert.That(result.Value[0].RoleClaims, Has.Count.EqualTo(1));
        Assert.That(result.Value[0].UserClaims, Has.Count.EqualTo(1));
    }
}
