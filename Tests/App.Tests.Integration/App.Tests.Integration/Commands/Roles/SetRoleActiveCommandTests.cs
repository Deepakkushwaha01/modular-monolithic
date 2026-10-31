using App.Core.Command.Identity.Roles;
using App.Core.Domain.Identity.Roles;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Commands.Roles;

public sealed class SetRoleActiveCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_ChangesActiveState()
    {
        (Role Entity, long Id) role = await SeedRoleAsync();

        Result result = await new SetRoleActiveCommandHandler(Roles, UnitOfWork)
            .ExecuteAsync(new(role.Entity.Uid, false));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That((await Roles.GetRoleByUidAsync(role.Entity.Uid)).Value.IsActive, Is.False);
    }

    [Test]
    public async Task ExecuteAsync_ActivatesInactiveRole()
    {
        (Role Entity, long Id) role = await SeedRoleAsync();
        await new SetRoleActiveCommandHandler(Roles, UnitOfWork)
            .ExecuteAsync(new(role.Entity.Uid, false));

        Result result = await new SetRoleActiveCommandHandler(Roles, UnitOfWork)
            .ExecuteAsync(new(role.Entity.Uid, true));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That((await Roles.GetRoleByUidAsync(role.Entity.Uid)).Value.IsActive, Is.True);
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownRole()
    {
        Result result = await new SetRoleActiveCommandHandler(Roles, UnitOfWork)
            .ExecuteAsync(new(Guid.NewGuid(), false));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }

    [Test]
    public async Task ExecuteAsync_CannotDeactivateSystemRole()
    {
        (Role Entity, long Id) role = await SeedRoleAsync("System", "system", isSystemRole: true);

        Result result = await new SetRoleActiveCommandHandler(Roles, UnitOfWork)
            .ExecuteAsync(new(role.Entity.Uid, false));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.Conflicted));
    }
}
