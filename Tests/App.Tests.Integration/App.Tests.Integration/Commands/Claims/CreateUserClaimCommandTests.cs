using App.Core.Command.Identity.Claims;
using App.Core.Domain.Identity.Claims;
using App.Core.Domain.Identity.Users;

namespace App.Tests.Integration.Commands.Claims;

public sealed class CreateUserClaimCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_CreatesUserClaim()
    {
        (User Entity, long Id) user = await SeedUserAsync();
        (ClaimType Entity, long Id) claimType = await SeedClaimTypeAsync();

        Result<Guid> result = await new CreateUserClaimCommandHandler(UserClaims, UnitOfWork)
            .ExecuteAsync(new(user.Entity.Uid, claimType.Entity.Uid, " premium "));

        Assert.That(result.IsSuccess, Is.True);
        Result<UserClaim> saved = await UserClaims.GetUserClaimByUidAsync(result.Value);
        Assert.That(saved.IsSuccess, Is.True);
        Assert.That(saved.Value.ClaimValue, Is.EqualTo("premium"));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsFailureWhenForeignKeyIsInvalid()
    {
        (ClaimType Entity, long Id) claimType = await SeedClaimTypeAsync();

        Result<Guid> result = await new CreateUserClaimCommandHandler(UserClaims, UnitOfWork)
            .ExecuteAsync(new(Guid.NewGuid(), claimType.Entity.Uid, "premium"));

        Assert.That(result.IsFailure, Is.True);
    }
}
