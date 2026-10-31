using App.Core.Command.Identity.Roles;
using App.Core.Domain.Identity.Roles;
using App.Core.Domain.Identity.Users;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Commands.Roles;

public sealed class DeleteUserRoleCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_DeletesExistingAssignment()
    {
        (User Entity, long Id) user = await SeedUserAsync();
        (Role Entity, long Id) role = await SeedRoleAsync();
        Assert.That((await UserRoles.InsertAsync(UserRole.Create(user.Entity.Uid, role.Entity.Uid))).IsSuccess, Is.True);
        Assert.That((await UnitOfWork.SaveChangesAsync()).IsSuccess, Is.True);
        UserRole assignment = (await UserRoles.GetUserRolesAsync()).Value.Single();

        Result result = await new DeleteUserRoleCommandHandler(UserRoles, UnitOfWork)
            .ExecuteAsync(new(assignment.Uid));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That((await UserRoles.GetUserRoleByUidAsync(assignment.Uid)).ResultType, Is.EqualTo(ResultType.NotFound));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownAssignment()
    {
        Result result = await new DeleteUserRoleCommandHandler(UserRoles, UnitOfWork)
            .ExecuteAsync(new(Guid.NewGuid()));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }
}
