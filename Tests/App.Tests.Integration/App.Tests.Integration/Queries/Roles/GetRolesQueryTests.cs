using App.Core.Domain.Identity.Claims;
using App.Core.Domain.Identity.Roles;
using App.Core.Domain.Identity.Users;
using App.Core.Queries.Identity.Roles;

namespace App.Tests.Integration.Queries.Roles;

public sealed class GetRolesQueryTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_ReturnsEmptyListWhenNoRolesExist()
    {
        Result<List<Role>> result = await new GetRolesQueryHandler(Roles).ExecuteAsync(new());

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Empty);
    }

    [Test]
    public async Task ExecuteAsync_IncludesRequestedUserAndClaimRelations()
    {
        (Role Entity, long Id) role = await SeedRoleAsync();
        (User Entity, long Id) user = await SeedUserAsync();
        (ClaimType Entity, long Id) claimType = await SeedClaimTypeAsync();
        await SeedUserRoleAsync(user.Entity.Uid, role.Entity.Uid);
        await SeedRoleClaimAsync(role.Entity.Uid, claimType.Entity.Uid);

        Result<List<Role>> result = await new GetRolesQueryHandler(Roles)
            .ExecuteAsync(new(IncludeUsers: true, IncludeClaims: true));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Has.Count.EqualTo(1));
        Assert.That(result.Value[0].UserRoles, Has.Count.EqualTo(1));
        Assert.That(result.Value[0].RoleClaims, Has.Count.EqualTo(1));
    }
}
