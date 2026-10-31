using App.Core.Command.Identity.Claims;
using App.Core.Domain.Identity.Claims;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Commands.Claims;

public sealed class DeleteClaimTypeCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_DeletesExistingClaimType()
    {
        (ClaimType Entity, long Id) seeded = await SeedClaimTypeAsync();

        Result result = await new DeleteClaimTypeCommandHandler(ClaimTypes, UnitOfWork)
            .ExecuteAsync(new(seeded.Entity.Uid));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That((await ClaimTypes.GetClaimTypeByUidAsync(seeded.Entity.Uid)).ResultType,
            Is.EqualTo(ResultType.NotFound));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownClaimType()
    {
        Result result = await new DeleteClaimTypeCommandHandler(ClaimTypes, UnitOfWork)
            .ExecuteAsync(new(Guid.NewGuid()));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }
}
