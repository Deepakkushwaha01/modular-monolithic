using App.Core.Command.Identity.Roles;
using App.Core.Domain.Identity.Roles;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Commands.Roles;

public sealed class DeleteRoleCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_DeletesRegularRole()
    {
        (Role Entity, long Id) role = await SeedRoleAsync();

        Result result = await new DeleteRoleCommandHandler(Roles, UnitOfWork)
            .ExecuteAsync(new(role.Entity.Uid));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That((await Roles.GetRoleByUidAsync(role.Entity.Uid)).ResultType, Is.EqualTo(ResultType.NotFound));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownRole()
    {
        Result result = await new DeleteRoleCommandHandler(Roles, UnitOfWork)
            .ExecuteAsync(new(Guid.NewGuid()));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }

    [Test]
    public async Task ExecuteAsync_RejectsSystemRoleDeletion()
    {
        (Role Entity, long Id) role = await SeedRoleAsync("System", "system", isSystemRole: true);

        Result result = await new DeleteRoleCommandHandler(Roles, UnitOfWork)
            .ExecuteAsync(new(role.Entity.Uid));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.Conflicted));
    }
}
