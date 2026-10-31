using App.Core.Domain.Identity.Roles;
using App.Core.Domain.Identity.Users;
using App.Core.Queries.Identity.Roles;

namespace App.Tests.Integration.Queries.Roles;

public sealed class GetUserRolesQueryTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_ReturnsEmptyListWhenNoAssignmentsExist()
    {
        Result<List<UserRole>> result = await new GetUserRolesQueryHandler(UserRoles).ExecuteAsync(new());

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Empty);
    }

    [Test]
    public async Task ExecuteAsync_ReturnsAllAssignments()
    {
        (User Entity, long Id) user = await SeedUserAsync();
        (Role Entity, long Id) role = await SeedRoleAsync();
        await SeedUserRoleAsync(user.Entity.Uid, role.Entity.Uid);

        Result<List<UserRole>> result = await new GetUserRolesQueryHandler(UserRoles).ExecuteAsync(new());

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Has.Count.EqualTo(1));
        Assert.That(result.Value[0].UserId, Is.EqualTo(user.Entity.Uid));
        Assert.That(result.Value[0].RoleId, Is.EqualTo(role.Entity.Uid));
    }
}
