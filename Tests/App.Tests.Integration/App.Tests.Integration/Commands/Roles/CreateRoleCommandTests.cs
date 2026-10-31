using App.Core.Command.Identity.Roles;
using App.Core.Domain.Identity.Roles;
using App.Core.Persistence.Identity.Roles.Repositories;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Commands.Roles;

public sealed class CreateRoleCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_NormalizesSlugAndOptionalDescription()
    {
        Result<Guid> result = await new CreateRoleCommandHandler(Roles, UnitOfWork)
            .ExecuteAsync(new("  Customer Support  ", null, "  Support users  "));

        Assert.That(result.IsSuccess, Is.True);
        Result<Role> saved = await new RoleRepository(Db).GetRoleByUidAsync(result.Value);
        Assert.That(saved.Value.Name, Is.EqualTo("Customer Support"));
        Assert.That(saved.Value.Slug, Is.EqualTo("customer-support"));
        Assert.That(saved.Value.Description, Is.EqualTo("Support users"));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsFailureForDuplicateSlug()
    {
        await SeedRoleAsync("First", "duplicate");

        Result<Guid> result = await new CreateRoleCommandHandler(Roles, UnitOfWork)
            .ExecuteAsync(new("Second", "duplicate", null));

        Assert.That(result.IsFailure, Is.True);
    }
}
