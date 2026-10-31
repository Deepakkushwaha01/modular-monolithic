using App.Core.Domain.Identity.Claims;
using App.Core.Domain.Identity.Roles;
using App.Core.Domain.Identity.Users;
using App.Core.Queries.Identity.Roles;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Queries.Roles;

public sealed class GetRoleByUidQueryTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_IncludesRequestedUserAndClaimRelations()
    {
        (Role Entity, long Id) role = await SeedRoleAsync();
        (User Entity, long Id) user = await SeedUserAsync();
        (ClaimType Entity, long Id) claimType = await SeedClaimTypeAsync();
        await SeedUserRoleAsync(user.Entity.Uid, role.Entity.Uid);
        await SeedRoleClaimAsync(role.Entity.Uid, claimType.Entity.Uid);

        Result<Role> result = await new GetRoleByUidQueryHandler(Roles)
            .ExecuteAsync(new(role.Entity.Uid, IncludeUsers: true, IncludeClaims: true));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.UserRoles, Has.Count.EqualTo(1));
        Assert.That(result.Value.RoleClaims, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownUid()
    {
        Result<Role> result = await new GetRoleByUidQueryHandler(Roles).ExecuteAsync(new(Guid.NewGuid()));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }
}
