using App.Core.Domain.Identity.Claims;
using App.Core.Domain.Identity.Roles;
using App.Core.Domain.Identity.Users;
using App.Core.Queries.Identity.Claims;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Queries.Claims;

public sealed class GetClaimTypeByUidQueryTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_ReturnsClaimAndRequestedRelatedClaims()
    {
        (ClaimType Entity, long Id) claimType = await SeedClaimTypeAsync();
        (Role Entity, long Id) role = await SeedRoleAsync();
        (User Entity, long Id) user = await SeedUserAsync();
        await SeedRoleClaimAsync(role.Entity.Uid, claimType.Entity.Uid);
        await SeedUserClaimAsync(user.Entity.Uid, claimType.Entity.Uid);

        Result<ClaimType> result = await new GetClaimTypeByUidQueryHandler(ClaimTypes)
            .ExecuteAsync(new(claimType.Entity.Uid, IncludeRoleClaims: true, IncludeUserClaims: true));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.RoleClaims, Has.Count.EqualTo(1));
        Assert.That(result.Value.UserClaims, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownUid()
    {
        Result<ClaimType> result = await new GetClaimTypeByUidQueryHandler(ClaimTypes).ExecuteAsync(new(Guid.NewGuid()));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }
}
