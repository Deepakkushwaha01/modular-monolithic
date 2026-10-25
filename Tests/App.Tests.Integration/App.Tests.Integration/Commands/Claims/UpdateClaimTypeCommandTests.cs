using App.Core.Command.Identity.Claims;
using App.Core.Domain.Identity.Claims;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Commands.Claims;

public sealed class UpdateClaimTypeCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_UpdatesClaimTypeAndReturnsItsUid()
    {
        var seeded = await SeedClaimTypeAsync();
        UpdateClaimTypeCommandHandler handler = new(ClaimTypes, UnitOfWork);

        var result = await handler.ExecuteAsync(new UpdateClaimTypeCommand(
            seeded.Entity.Uid, " Updated ", " description ", ClaimDataType.Boolean, " Access ", false));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.EqualTo(seeded.Entity.Uid));
        var saved = await ClaimTypes.GetClaimTypeByUidAsync(seeded.Entity.Uid);
        Assert.That(saved.Value.Name, Is.EqualTo("Updated"));
        Assert.That(saved.Value.Description, Is.EqualTo("description"));
        Assert.That(saved.Value.Module, Is.EqualTo("Access"));
        Assert.That(saved.Value.IsActive, Is.False);
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownClaimType()
    {
        var result = await new UpdateClaimTypeCommandHandler(ClaimTypes, UnitOfWork)
            .ExecuteAsync(new(Guid.NewGuid(), "Name", null, ClaimDataType.String, "Tests", true));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }
}
