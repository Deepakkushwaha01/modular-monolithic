using App.Core.Command.Identity.Roles;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Commands.Roles;

public sealed class UpdateRoleCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_UpdatesRegularRole()
    {
        var role = await SeedRoleAsync();

        var result = await new UpdateRoleCommandHandler(Roles, UnitOfWork)
            .ExecuteAsync(new(role.Entity.Uid, " New Role ", null, " New description ", false));

        Assert.That(result.IsSuccess, Is.True);
        var saved = await Roles.GetRoleByUidAsync(role.Entity.Uid);
        Assert.That(saved.Value.Name, Is.EqualTo("New Role"));
        Assert.That(saved.Value.Slug, Is.EqualTo("new-role"));
        Assert.That(saved.Value.Description, Is.EqualTo("New description"));
        Assert.That(saved.Value.IsActive, Is.False);

        var activate = await new UpdateRoleCommandHandler(Roles, UnitOfWork)
            .ExecuteAsync(new(role.Entity.Uid, "New Role", null, null, true));

        Assert.That(activate.IsSuccess, Is.True);
        Assert.That((await Roles.GetRoleByUidAsync(role.Entity.Uid)).Value.IsActive, Is.True);
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownRole()
    {
        var result = await new UpdateRoleCommandHandler(Roles, UnitOfWork)
            .ExecuteAsync(new(Guid.NewGuid(), "Name", null, null, true));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }

    [Test]
    public async Task ExecuteAsync_RejectsSystemRoleChanges()
    {
        var role = await SeedRoleAsync("System", "system", isSystemRole: true);

        var result = await new UpdateRoleCommandHandler(Roles, UnitOfWork)
            .ExecuteAsync(new(role.Entity.Uid, "Changed", "changed", null, true));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.Conflicted));
    }
}
