using App.Core.Queries.Identity.Users;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Queries.Users;

public sealed class GetUserByUidQueryTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_IncludesRequestedAddressesRolesAndClaims()
    {
        var user = await SeedUserAsync();
        var role = await SeedRoleAsync();
        var claimType = await SeedClaimTypeAsync();
        await SeedAddressAsync(user.Id);
        await SeedUserRoleAsync(user.Id, role.Id);
        await SeedUserClaimAsync(user.Id, claimType.Id);

        var result = await new GetUserByUidQueryHandler(Users)
            .ExecuteAsync(new(user.Entity.Uid, IncludeAddresses: true, IncludeRoles: true, IncludeClaims: true));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.Addresses, Has.Count.EqualTo(1));
        Assert.That(result.Value.UserRoles, Has.Count.EqualTo(1));
        Assert.That(result.Value.UserClaims, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownUid()
    {
        var result = await new GetUserByUidQueryHandler(Users).ExecuteAsync(new(Guid.NewGuid()));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }
}
