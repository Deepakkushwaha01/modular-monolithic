using App.Core.Domain.Identity.Claims;
using App.Core.Domain.Identity.Roles;
using App.Core.Domain.Identity.Users;
using App.Core.Queries.Identity.Users;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Queries.Users;

public sealed class GetUserByUidQueryTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_IncludesRequestedAddressesRolesAndClaims()
    {
        (User Entity, long Id) user = await SeedUserAsync();
        (Role Entity, long Id) role = await SeedRoleAsync();
        (ClaimType Entity, long Id) claimType = await SeedClaimTypeAsync();
        await SeedAddressAsync(user.Id);
        await SeedUserRoleAsync(user.Entity.Uid, role.Entity.Uid);
        await SeedUserClaimAsync(user.Entity.Uid, claimType.Entity.Uid);

        Result<User> result = await new GetUserByUidQueryHandler(Users)
            .ExecuteAsync(new(user.Entity.Uid, IncludeAddresses: true, IncludeRoles: true, IncludeClaims: true));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.Addresses, Has.Count.EqualTo(1));
        Assert.That(result.Value.UserRoles, Has.Count.EqualTo(1));
        Assert.That(result.Value.UserClaims, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownUid()
    {
        Result<User> result = await new GetUserByUidQueryHandler(Users).ExecuteAsync(new(Guid.NewGuid()));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }
}
