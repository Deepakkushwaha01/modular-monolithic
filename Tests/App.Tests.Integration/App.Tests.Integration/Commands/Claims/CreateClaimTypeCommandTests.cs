using App.Core.Command.Identity.Claims;
using App.Core.Domain.Identity.Claims;

namespace App.Tests.Integration.Commands.Claims;

public sealed class CreateClaimTypeCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_TrimsValuesAndPersistsClaimType()
    {
        CreateClaimTypeCommandHandler handler = new(ClaimTypes, UnitOfWork);

        var result = await handler.ExecuteAsync(new CreateClaimTypeCommand(
            "  PreferredName  ", "  User profile  ", ClaimDataType.String, "  Identity  "));

        Assert.That(result.IsSuccess, Is.True);
        var saved = await new App.Core.Queries.Identity.Claims.GetClaimTypeByUidQueryHandler(ClaimTypes)
            .ExecuteAsync(new(result.Value));
        Assert.That(saved.IsSuccess, Is.True);
        Assert.That(saved.Value.Name, Is.EqualTo("PreferredName"));
        Assert.That(saved.Value.Description, Is.EqualTo("User profile"));
        Assert.That(saved.Value.Module, Is.EqualTo("Identity"));
    }

    [Test]
    public async Task ExecuteAsync_MapsDatabaseConstraintFailureToFailedResult()
    {
        await SeedClaimTypeAsync("UniqueClaim");
        CreateClaimTypeCommandHandler handler = new(ClaimTypes, UnitOfWork);

        var result = await handler.ExecuteAsync(new CreateClaimTypeCommand(
            "UniqueClaim", null, ClaimDataType.Boolean, "Tests"));

        Assert.That(result.IsFailure, Is.True);
    }
}
