using App.Core.Domain.Identity.Claims;
using App.Core.Queries.Identity.Claims;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Queries.Claims;

public sealed class GetRoleClaimByUidQueryTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_ReturnsRoleClaim()
    {
        var role = await SeedRoleAsync();
        var claimType = await SeedClaimTypeAsync();
        await SeedRoleClaimAsync(role.Id, claimType.Id, "admin");
        var claim = (await RoleClaims.GetRoleClaimsAsync()).Value.Single();

        var result = await new GetRoleClaimByUidQueryHandler(RoleClaims).ExecuteAsync(new(claim.Uid));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.ClaimValue, Is.EqualTo("admin"));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownUid()
    {
        var result = await new GetRoleClaimByUidQueryHandler(RoleClaims).ExecuteAsync(new(Guid.NewGuid()));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }
}
