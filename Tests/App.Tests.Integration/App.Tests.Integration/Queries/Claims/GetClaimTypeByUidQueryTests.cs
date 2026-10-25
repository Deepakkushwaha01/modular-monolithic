using App.Core.Domain.Identity.Claims;
using App.Core.Queries.Identity.Claims;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Queries.Claims;

public sealed class GetClaimTypeByUidQueryTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_ReturnsClaimAndRequestedRelatedClaims()
    {
        var claimType = await SeedClaimTypeAsync();
        var role = await SeedRoleAsync();
        var user = await SeedUserAsync();
        await SeedRoleClaimAsync(role.Id, claimType.Id);
        await SeedUserClaimAsync(user.Id, claimType.Id);

        var result = await new GetClaimTypeByUidQueryHandler(ClaimTypes)
            .ExecuteAsync(new(claimType.Entity.Uid, IncludeRoleClaims: true, IncludeUserClaims: true));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.RoleClaims, Has.Count.EqualTo(1));
        Assert.That(result.Value.UserClaims, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownUid()
    {
        var result = await new GetClaimTypeByUidQueryHandler(ClaimTypes).ExecuteAsync(new(Guid.NewGuid()));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }
}
