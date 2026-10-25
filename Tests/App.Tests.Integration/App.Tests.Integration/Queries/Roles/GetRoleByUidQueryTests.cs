using App.Core.Queries.Identity.Roles;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Queries.Roles;

public sealed class GetRoleByUidQueryTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_IncludesRequestedUserAndClaimRelations()
    {
        var role = await SeedRoleAsync();
        var user = await SeedUserAsync();
        var claimType = await SeedClaimTypeAsync();
        await SeedUserRoleAsync(user.Id, role.Id);
        await SeedRoleClaimAsync(role.Id, claimType.Id);

        var result = await new GetRoleByUidQueryHandler(Roles)
            .ExecuteAsync(new(role.Entity.Uid, IncludeUsers: true, IncludeClaims: true));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.UserRoles, Has.Count.EqualTo(1));
        Assert.That(result.Value.RoleClaims, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownUid()
    {
        var result = await new GetRoleByUidQueryHandler(Roles).ExecuteAsync(new(Guid.NewGuid()));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }
}
