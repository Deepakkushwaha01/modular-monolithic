using App.Core.Domain.Identity.Claims;
using App.Core.Domain.Identity.Roles;
using App.Core.Domain.Identity.Users;
using App.Core.Domain.Identity.Addresses;
using App.Core.Persistence.Configurations.Database.Context;
using App.Core.Persistence.Configurations.Database.Context.UnitOfWork;
using App.Core.Persistence.Identity.Addresses.Repositories;
using App.Core.Persistence.Identity.Claims.Repositories;
using App.Core.Persistence.Identity.Roles.Repositories;
using App.Core.Persistence.Identity.Users.Repositories;
using Microsoft.EntityFrameworkCore;
using PersistenceClaimType = App.Core.Persistence.Identity.Claims.Entities.ClaimType;
using PersistenceRole = App.Core.Persistence.Identity.Roles.Entities.Role;
using PersistenceUser = App.Core.Persistence.Identity.Users.Entities.User;

namespace App.Tests.Integration;

[NonParallelizable]
public abstract class IntegrationTestBase
{
    protected AppDbContext Db { get; private set; } = null!;
    protected UnitOfWork UnitOfWork { get; private set; } = null!;
    protected ClaimTypeRepository ClaimTypes => new(Db);
    protected RoleClaimRepository RoleClaims => new(Db);
    protected UserClaimRepository UserClaims => new(Db);
    protected RoleRepository Roles => new(Db);
    protected UserRoleRepository UserRoles => new(Db);
    protected UserRepository Users => new(Db);
    protected AddressRepository Addresses => new(Db);

    [SetUp]
    public async Task SetUpDatabaseAsync()
    {
        DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(PostgreSqlTestDatabase.ConnectionString)
            .Options;

        Db = new AppDbContext(options, new AppDbContextSeedOptions { LoadSeedData = false });
        await Db.Database.ExecuteSqlRawAsync("DROP SCHEMA IF EXISTS \"Identity\" CASCADE");
        await Db.Database.EnsureCreatedAsync();

        UnitOfWork = new UnitOfWork(Db);
    }

    [TearDown]
    public async Task DisposeDatabaseAsync() => await Db.DisposeAsync();

    protected async Task<(User Entity, long Id)> SeedUserAsync(
        string email = "person@example.com",
        string phone = "+14155552671")
    {
        User user = User.Create(email, "test-password-hash", phone, "Test", "User");
        Assert.That((await Users.InsertAsync(user)).IsSuccess, Is.True);
        Assert.That((await UnitOfWork.SaveChangesAsync()).IsSuccess, Is.True);
        long id = await Db.Set<PersistenceUser>()
            .Where(entity => entity.Uid == user.Uid)
            .Select(entity => entity.Id)
            .SingleAsync();
        return (user, id);
    }

    protected async Task<(Role Entity, long Id)> SeedRoleAsync(
        string name = "Test Role",
        string slug = "test-role",
        bool isSystemRole = false)
    {
        Role role = Role.Create(name, slug, "Test role", isSystemRole);
        Assert.That((await Roles.InsertAsync(role)).IsSuccess, Is.True);
        Assert.That((await UnitOfWork.SaveChangesAsync()).IsSuccess, Is.True);
        long id = await Db.Set<PersistenceRole>()
            .Where(entity => entity.Uid == role.Uid)
            .Select(entity => entity.Id)
            .SingleAsync();
        return (role, id);
    }

    protected async Task<(ClaimType Entity, long Id)> SeedClaimTypeAsync(
        string name = "TestClaim",
        string module = "Tests")
    {
        ClaimType claimType = ClaimType.Create(name, "Test claim", ClaimDataType.String, module);
        Assert.That((await ClaimTypes.InsertAsync(claimType)).IsSuccess, Is.True);
        Assert.That((await UnitOfWork.SaveChangesAsync()).IsSuccess, Is.True);
        long id = await Db.Set<PersistenceClaimType>()
            .Where(entity => entity.Uid == claimType.Uid)
            .Select(entity => entity.Id)
            .SingleAsync();
        return (claimType, id);
    }

    protected async Task SeedRoleClaimAsync(Guid roleUid, Guid claimTypeUid, string value = "true")
    {
        Assert.That((await RoleClaims.InsertAsync(RoleClaim.Create(roleUid, claimTypeUid, value))).IsSuccess, Is.True);
        Assert.That((await UnitOfWork.SaveChangesAsync()).IsSuccess, Is.True);
    }

    protected async Task SeedUserClaimAsync(Guid userUid, Guid claimTypeUid, string value = "true")
    {
        Assert.That((await UserClaims.InsertAsync(UserClaim.Create(userUid, claimTypeUid, value))).IsSuccess, Is.True);
        Assert.That((await UnitOfWork.SaveChangesAsync()).IsSuccess, Is.True);
    }

    protected async Task SeedUserRoleAsync(Guid userUid, Guid roleUid)
    {
        Assert.That((await UserRoles.InsertAsync(UserRole.Create(userUid, roleUid))).IsSuccess, Is.True);
        Assert.That((await UnitOfWork.SaveChangesAsync()).IsSuccess, Is.True);
    }

    protected async Task SeedAddressAsync(long userId)
    {
        Address address = Address.Create(userId, "Home", "123 Test Street", null, "Test City", "Test State",
            "12345", "US", true);
        Assert.That((await Addresses.InsertAsync(address)).IsSuccess, Is.True);
        Assert.That((await UnitOfWork.SaveChangesAsync()).IsSuccess, Is.True);
    }
}
