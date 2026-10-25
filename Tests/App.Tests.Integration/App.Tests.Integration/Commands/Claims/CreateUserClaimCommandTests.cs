using App.Core.Command.Identity.Claims;
using App.Core.Domain.Identity.Claims;

namespace App.Tests.Integration.Commands.Claims;

public sealed class CreateUserClaimCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_CreatesUserClaim()
    {
        var user = await SeedUserAsync();
        var claimType = await SeedClaimTypeAsync();

        var result = await new CreateUserClaimCommandHandler(UserClaims, UnitOfWork)
            .ExecuteAsync(new(user.Id, claimType.Id, " premium "));

        Assert.That(result.IsSuccess, Is.True);
        var saved = await UserClaims.GetUserClaimByUidAsync(result.Value);
        Assert.That(saved.IsSuccess, Is.True);
        Assert.That(saved.Value.ClaimValue, Is.EqualTo("premium"));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsFailureWhenForeignKeyIsInvalid()
    {
        var claimType = await SeedClaimTypeAsync();

        var result = await new CreateUserClaimCommandHandler(UserClaims, UnitOfWork)
            .ExecuteAsync(new(long.MaxValue, claimType.Id, "premium"));

        Assert.That(result.IsFailure, Is.True);
    }
}
