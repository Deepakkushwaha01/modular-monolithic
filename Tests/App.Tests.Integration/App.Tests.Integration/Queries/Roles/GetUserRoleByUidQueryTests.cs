using App.Core.Domain.Identity.Roles;
using App.Core.Queries.Identity.Roles;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Queries.Roles;

public sealed class GetUserRoleByUidQueryTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_ReturnsUserRole()
    {
        var user = await SeedUserAsync();
        var role = await SeedRoleAsync();
        await SeedUserRoleAsync(user.Id, role.Id);
        var assignment = (await UserRoles.GetUserRolesAsync()).Value.Single();

        var result = await new GetUserRoleByUidQueryHandler(UserRoles).ExecuteAsync(new(assignment.Uid));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.UserId, Is.EqualTo(user.Id));
        Assert.That(result.Value.RoleId, Is.EqualTo(role.Id));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownUid()
    {
        var result = await new GetUserRoleByUidQueryHandler(UserRoles).ExecuteAsync(new(Guid.NewGuid()));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }
}
