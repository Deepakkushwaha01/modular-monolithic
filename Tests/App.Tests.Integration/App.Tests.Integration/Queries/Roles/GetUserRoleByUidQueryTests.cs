using App.Core.Domain.Identity.Roles;
using App.Core.Domain.Identity.Users;
using App.Core.Queries.Identity.Roles;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Queries.Roles;

public sealed class GetUserRoleByUidQueryTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_ReturnsUserRole()
    {
        (User Entity, long Id) user = await SeedUserAsync();
        (Role Entity, long Id) role = await SeedRoleAsync();
        await SeedUserRoleAsync(user.Entity.Uid, role.Entity.Uid);
        UserRole assignment = (await UserRoles.GetUserRolesAsync()).Value.Single();

        Result<UserRole> result = await new GetUserRoleByUidQueryHandler(UserRoles).ExecuteAsync(new(assignment.Uid));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.UserId, Is.EqualTo(user.Entity.Uid));
        Assert.That(result.Value.RoleId, Is.EqualTo(role.Entity.Uid));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownUid()
    {
        Result<UserRole> result = await new GetUserRoleByUidQueryHandler(UserRoles).ExecuteAsync(new(Guid.NewGuid()));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }
}
