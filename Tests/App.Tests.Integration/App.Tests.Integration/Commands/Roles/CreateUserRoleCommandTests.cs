using App.Core.Command.Identity.Roles;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Commands.Roles;

public sealed class CreateUserRoleCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_CreatesAssignmentAndAllowsNullAssigner()
    {
        var user = await SeedUserAsync();
        var role = await SeedRoleAsync();

        var result = await new CreateUserRoleCommandHandler(UserRoles, UnitOfWork)
            .ExecuteAsync(new(user.Id, role.Id, null));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That((await UserRoles.GetUserRoleByUidAsync(result.Value)).IsSuccess, Is.True);
    }

    [TestCase(0, 1, null, ResultType.Invalid)]
    [TestCase(1, 0, null, ResultType.Invalid)]
    [TestCase(1, 1, 0, ResultType.Invalid)]
    public async Task ExecuteAsync_RejectsNonPositiveIds(long userId, long roleId, long? assignedBy, ResultType expected)
    {
        var result = await new CreateUserRoleCommandHandler(UserRoles, UnitOfWork)
            .ExecuteAsync(new(userId, roleId, assignedBy));

        Assert.That(result.ResultType, Is.EqualTo(expected));
    }
}
