using App.Core.Command.Identity.Roles;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Commands.Roles;

public sealed class DeleteRoleCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_DeletesRegularRole()
    {
        var role = await SeedRoleAsync();

        var result = await new DeleteRoleCommandHandler(Roles, UnitOfWork)
            .ExecuteAsync(new(role.Entity.Uid));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That((await Roles.GetRoleByUidAsync(role.Entity.Uid)).ResultType, Is.EqualTo(ResultType.NotFound));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownRole()
    {
        var result = await new DeleteRoleCommandHandler(Roles, UnitOfWork)
            .ExecuteAsync(new(Guid.NewGuid()));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }

    [Test]
    public async Task ExecuteAsync_RejectsSystemRoleDeletion()
    {
        var role = await SeedRoleAsync("System", "system", isSystemRole: true);

        var result = await new DeleteRoleCommandHandler(Roles, UnitOfWork)
            .ExecuteAsync(new(role.Entity.Uid));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.Conflicted));
    }
}
