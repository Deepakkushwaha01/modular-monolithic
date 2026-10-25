using App.Core.Command.Identity.Roles;
using App.Core.Domain.Identity.Roles;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Commands.Roles;

public sealed class DeleteUserRoleCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_DeletesExistingAssignment()
    {
        var user = await SeedUserAsync();
        var role = await SeedRoleAsync();
        Assert.That((await UserRoles.InsertAsync(UserRole.Create(user.Id, role.Id))).IsSuccess, Is.True);
        Assert.That((await UnitOfWork.SaveChangesAsync()).IsSuccess, Is.True);
        var assignment = (await UserRoles.GetUserRolesAsync()).Value.Single();

        var result = await new DeleteUserRoleCommandHandler(UserRoles, UnitOfWork)
            .ExecuteAsync(new(assignment.Uid));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That((await UserRoles.GetUserRoleByUidAsync(assignment.Uid)).ResultType, Is.EqualTo(ResultType.NotFound));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownAssignment()
    {
        var result = await new DeleteUserRoleCommandHandler(UserRoles, UnitOfWork)
            .ExecuteAsync(new(Guid.NewGuid()));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }
}
