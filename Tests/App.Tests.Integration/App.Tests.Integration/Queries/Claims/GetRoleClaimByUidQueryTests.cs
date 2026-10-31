using App.Core.Domain.Identity.Claims;
using App.Core.Domain.Identity.Roles;
using App.Core.Queries.Identity.Claims;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Queries.Claims;

public sealed class GetRoleClaimByUidQueryTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_ReturnsRoleClaim()
    {
        (Role Entity, long Id) role = await SeedRoleAsync();
        (ClaimType Entity, long Id) claimType = await SeedClaimTypeAsync();
        await SeedRoleClaimAsync(role.Entity.Uid, claimType.Entity.Uid, "admin");
        RoleClaim claim = (await RoleClaims.GetRoleClaimsAsync()).Value.Single();

        Result<RoleClaim> result = await new GetRoleClaimByUidQueryHandler(RoleClaims).ExecuteAsync(new(claim.Uid));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.ClaimValue, Is.EqualTo("admin"));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownUid()
    {
        Result<RoleClaim> result = await new GetRoleClaimByUidQueryHandler(RoleClaims).ExecuteAsync(new(Guid.NewGuid()));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }
}
