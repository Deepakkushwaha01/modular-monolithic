using App.Core.Queries.Identity.Roles;

namespace App.Tests.Integration.Queries.Roles;

public sealed class GetUserRolesQueryTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_ReturnsEmptyListWhenNoAssignmentsExist()
    {
        var result = await new GetUserRolesQueryHandler(UserRoles).ExecuteAsync(new());

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Empty);
    }

    [Test]
    public async Task ExecuteAsync_ReturnsAllAssignments()
    {
        var user = await SeedUserAsync();
        var role = await SeedRoleAsync();
        await SeedUserRoleAsync(user.Id, role.Id);

        var result = await new GetUserRolesQueryHandler(UserRoles).ExecuteAsync(new());

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Has.Count.EqualTo(1));
        Assert.That(result.Value[0].UserId, Is.EqualTo(user.Id));
        Assert.That(result.Value[0].RoleId, Is.EqualTo(role.Id));
    }
}
