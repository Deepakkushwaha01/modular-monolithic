using App.Core.Queries.Identity.Roles;

namespace App.Tests.Integration.Queries.Roles;

public sealed class GetRolesQueryTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_ReturnsEmptyListWhenNoRolesExist()
    {
        var result = await new GetRolesQueryHandler(Roles).ExecuteAsync(new());

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Empty);
    }

    [Test]
    public async Task ExecuteAsync_IncludesRequestedUserAndClaimRelations()
    {
        var role = await SeedRoleAsync();
        var user = await SeedUserAsync();
        var claimType = await SeedClaimTypeAsync();
        await SeedUserRoleAsync(user.Id, role.Id);
        await SeedRoleClaimAsync(role.Id, claimType.Id);

        var result = await new GetRolesQueryHandler(Roles)
            .ExecuteAsync(new(IncludeUsers: true, IncludeClaims: true));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Has.Count.EqualTo(1));
        Assert.That(result.Value[0].UserRoles, Has.Count.EqualTo(1));
        Assert.That(result.Value[0].RoleClaims, Has.Count.EqualTo(1));
    }
}
