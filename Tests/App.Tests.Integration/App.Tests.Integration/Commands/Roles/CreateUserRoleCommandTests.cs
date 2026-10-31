using App.Core.Command.Identity.Roles;
using App.Core.Domain.Identity.Roles;
using App.Core.Domain.Identity.Users;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Commands.Roles;

public sealed class CreateUserRoleCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_CreatesAssignmentAndAllowsNullAssigner()
    {
        (User Entity, long Id) user = await SeedUserAsync();
        (Role Entity, long Id) role = await SeedRoleAsync();

        Result<Guid> result = await new CreateUserRoleCommandHandler(UserRoles, UnitOfWork)
            .ExecuteAsync(new(user.Entity.Uid, role.Entity.Uid, null));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That((await UserRoles.GetUserRoleByUidAsync(result.Value)).IsSuccess, Is.True);
    }

    public async Task ExecuteAsync_RejectsEmptyUids()
    {
        Result<Guid> result = await new CreateUserRoleCommandHandler(UserRoles, UnitOfWork)
            .ExecuteAsync(new(Guid.Empty, Guid.NewGuid(), null));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.Invalid));

        result = await new CreateUserRoleCommandHandler(UserRoles, UnitOfWork)
            .ExecuteAsync(new(Guid.NewGuid(), Guid.Empty, null));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.Invalid));

        result = await new CreateUserRoleCommandHandler(UserRoles, UnitOfWork)
            .ExecuteAsync(new(Guid.NewGuid(), Guid.NewGuid(), Guid.Empty));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.Invalid));
    }
}
