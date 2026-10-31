using App.Core.Domain.Identity.Claims;
using App.Core.Domain.Identity.Roles;
using App.Core.Domain.Identity.Users;
using App.Core.Queries.Identity.Claims;

namespace App.Tests.Integration.Queries.Claims;

public sealed class GetClaimTypesQueryTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_ReturnsEmptyListWhenNoClaimTypesExist()
    {
        Result<List<ClaimType>> result = await new GetClaimTypesQueryHandler(ClaimTypes).ExecuteAsync(new());

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Empty);
    }

    [Test]
    public async Task ExecuteAsync_IncludesRequestedRelatedClaims()
    {
        (ClaimType Entity, long Id) claimType = await SeedClaimTypeAsync();
        (Role Entity, long Id) role = await SeedRoleAsync();
        (User Entity, long Id) user = await SeedUserAsync();
        await SeedRoleClaimAsync(role.Entity.Uid, claimType.Entity.Uid);
        await SeedUserClaimAsync(user.Entity.Uid, claimType.Entity.Uid);

        Result<List<ClaimType>> result = await new GetClaimTypesQueryHandler(ClaimTypes)
            .ExecuteAsync(new(IncludeRoleClaims: true, IncludeUserClaims: true));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Has.Count.EqualTo(1));
        Assert.That(result.Value[0].RoleClaims, Has.Count.EqualTo(1));
        Assert.That(result.Value[0].UserClaims, Has.Count.EqualTo(1));
    }
}
