using App.Core.Command.Identity.Roles;
using App.Core.Domain.Identity.Roles;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Commands.Roles;

public sealed class UpdateUserRoleCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_UpdatesAssigner()
    {
        var user = await SeedUserAsync();
        var assigner = await SeedUserAsync("assigner@example.com");
        var role = await SeedRoleAsync();
        Assert.That((await UserRoles.InsertAsync(UserRole.Create(user.Id, role.Id, user.Id))).IsSuccess, Is.True);
        Assert.That((await UnitOfWork.SaveChangesAsync()).IsSuccess, Is.True);
        var assignment = (await UserRoles.GetUserRolesAsync()).Value.Single();

        var result = await new UpdateUserRoleCommandHandler(UserRoles, UnitOfWork)
            .ExecuteAsync(new(assignment.Uid, assigner.Id));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That((await UserRoles.GetUserRoleByUidAsync(assignment.Uid)).Value.AssignedBy, Is.EqualTo(assigner.Id));
    }

    [Test]
    public async Task ExecuteAsync_RejectsNonPositiveAssigner()
    {
        var result = await new UpdateUserRoleCommandHandler(UserRoles, UnitOfWork)
            .ExecuteAsync(new(Guid.NewGuid(), 0));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.Invalid));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownAssignment()
    {
        var result = await new UpdateUserRoleCommandHandler(UserRoles, UnitOfWork)
            .ExecuteAsync(new(Guid.NewGuid(), 2));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }
}
