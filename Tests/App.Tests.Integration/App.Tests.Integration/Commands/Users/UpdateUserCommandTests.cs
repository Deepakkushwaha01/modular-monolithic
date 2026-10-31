using App.Core.Command.Identity.Users;
using App.Core.Domain.Identity.Users;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Commands.Users;

public sealed class UpdateUserCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_UpdatesProfileAndNormalizesEmailAndPhone()
    {
        (User Entity, long Id) user = await SeedUserAsync();

        Result<Guid> result = await new UpdateUserCommandHandler(Users, UnitOfWork)
            .ExecuteAsync(new(user.Entity.Uid, " NEW@Example.com ", "+14155552672", " New ", " Name "));

        Assert.That(result.IsSuccess, Is.True);
        Result<User> saved = await Users.GetUserByUidAsync(user.Entity.Uid);
        Assert.That(saved.Value.Email, Is.EqualTo("new@example.com"));
        Assert.That(saved.Value.Phone, Is.EqualTo("+14155552672"));
        Assert.That(saved.Value.FirstName, Is.EqualTo("New"));
        Assert.That(saved.Value.LastName, Is.EqualTo("Name"));
        Assert.That(saved.Value.IsEmailVerified, Is.False);
        Assert.That(saved.Value.IsPhoneVerified, Is.False);
    }

    [TestCase("invalid", "+14155552671")]
    [TestCase("valid@example.com", "not-a-phone")]
    [TestCase("valid@example.com", "")]
    public async Task ExecuteAsync_RejectsInvalidEmailOrPhone(string email, string phone)
    {
        (User Entity, long Id) user = await SeedUserAsync();

        Result<Guid> result = await new UpdateUserCommandHandler(Users, UnitOfWork)
            .ExecuteAsync(new(user.Entity.Uid, email, phone, null, null));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.Invalid));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownUser()
    {
        Result<Guid> result = await new UpdateUserCommandHandler(Users, UnitOfWork)
            .ExecuteAsync(new(Guid.NewGuid(), "user@example.com", "+14155552671", null, null));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }

    [Test]
    public async Task ExecuteAsync_RejectsEmailAlreadyUsedByAnotherUser()
    {
        await SeedUserAsync("existing@example.com");
        (User Entity, long Id) user = await SeedUserAsync("current@example.com");

        Result<Guid> result = await new UpdateUserCommandHandler(Users, UnitOfWork)
            .ExecuteAsync(new(user.Entity.Uid, "existing@example.com", "+14155552671", null, null));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.Conflicted));
    }
}
