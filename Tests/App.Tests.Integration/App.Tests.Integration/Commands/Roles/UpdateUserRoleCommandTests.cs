using App.Core.Command.Identity.Roles;
using App.Core.Domain.Identity.Roles;
using App.Core.Domain.Identity.Users;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Commands.Roles;

public sealed class UpdateUserRoleCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_UpdatesAssigner()
    {
        (User Entity, long Id) user = await SeedUserAsync();
        (User Entity, long Id) assigner = await SeedUserAsync("assigner@example.com");
        (Role Entity, long Id) role = await SeedRoleAsync();
        Assert.That((await UserRoles.InsertAsync(UserRole.Create(user.Entity.Uid, role.Entity.Uid, user.Entity.Uid))).IsSuccess, Is.True);
        Assert.That((await UnitOfWork.SaveChangesAsync()).IsSuccess, Is.True);
        UserRole assignment = (await UserRoles.GetUserRolesAsync()).Value.Single();

        Result<Guid> result = await new UpdateUserRoleCommandHandler(UserRoles, UnitOfWork)
            .ExecuteAsync(new(assignment.Uid, assigner.Entity.Uid));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That((await UserRoles.GetUserRoleByUidAsync(assignment.Uid)).Value.AssignedBy, Is.EqualTo(assigner.Entity.Uid));
    }

    [Test]
    public async Task ExecuteAsync_RejectsEmptyAssignerUid()
    {
        Result<Guid> result = await new UpdateUserRoleCommandHandler(UserRoles, UnitOfWork)
            .ExecuteAsync(new(Guid.NewGuid(), Guid.Empty));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.Invalid));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownAssignment()
    {
        Result<Guid> result = await new UpdateUserRoleCommandHandler(UserRoles, UnitOfWork)
            .ExecuteAsync(new(Guid.NewGuid(), Guid.NewGuid()));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }
}
