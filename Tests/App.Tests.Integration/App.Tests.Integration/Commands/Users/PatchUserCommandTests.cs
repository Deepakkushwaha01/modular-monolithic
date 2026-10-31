using App.Core.Command.Identity.Users;
using App.Core.SharedLibrary.Errors;
using App.Core.SharedLibrary.Configuration.Identity;
using App.Core.Domain.Identity.Users;

namespace App.Tests.Integration.Commands.Users;

public sealed class PatchUserCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_UpdatesOnlySpecifiedFields()
    {
        (User Entity, long Id) user = await SeedUserAsync();
        Result<Guid> result = await new PatchUserCommandHandler(Users, UnitOfWork).ExecuteAsync(
            new(user.Entity.Uid, null, " Updated ", null, null, null, false, true, false, false));

        Assert.That(result.IsSuccess, Is.True);
        Result<User> saved = await Users.GetUserByUidAsync(user.Entity.Uid);
        Assert.That(saved.Value.FirstName, Is.EqualTo("Updated"));
        Assert.That(saved.Value.LastName, Is.EqualTo("User"));
        Assert.That(saved.Value.Email, Is.EqualTo("person@example.com"));
    }

    [Test]
    public async Task ExecuteAsync_RejectsWhenNoFieldsWereSpecified()
    {
        (User Entity, long Id) user = await SeedUserAsync();
        Result<Guid> result = await new PatchUserCommandHandler(Users, UnitOfWork).ExecuteAsync(
            new(user.Entity.Uid, null, null, null, null, null, false, false, false, false));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.Invalid));
    }

    [Test]
    public async Task ExecuteAsync_RejectsInvalidEmailAndPhone()
    {
        (User Entity, long Id) user = await SeedUserAsync();
        var handler = new PatchUserCommandHandler(Users, UnitOfWork);

        Result<Guid> invalidEmail = await handler.ExecuteAsync(
            new(user.Entity.Uid, "bad-email", null, null, null, null, true, false, false, false));
        Result<Guid> invalidPhone = await handler.ExecuteAsync(
            new(user.Entity.Uid, null, null, null, "bad-phone", null, false, false, false, true));

        Assert.That(invalidEmail.ResultType, Is.EqualTo(ResultType.Invalid));
        Assert.That(invalidPhone.ResultType, Is.EqualTo(ResultType.Invalid));
    }

    [Test]
    public async Task ExecuteAsync_RejectsFieldsOverConfiguredLimits()
    {
        (User Entity, long Id) user = await SeedUserAsync();
        var handler = new PatchUserCommandHandler(Users, UnitOfWork);
        Result<Guid> longFirstName = await handler.ExecuteAsync(new(
            user.Entity.Uid, null, new string('a', UserConfigurations.FirstNameMaximumLength + 1),
            null, null, null, false, true, false, false));
        Result<Guid> longLastName = await handler.ExecuteAsync(new(
            user.Entity.Uid, null, null, new string('a', UserConfigurations.LastNameMaximumLength + 1),
            null, null, false, false, true, false));
        Result<Guid> longPhoto = await handler.ExecuteAsync(new(
            user.Entity.Uid, null, null, null, null,
            new string('a', UserConfigurations.ProfilePhotoMaximumLength + 1),
            false, false, false, false));
        Result<Guid> longPhone = await handler.ExecuteAsync(new(
            user.Entity.Uid, null, null, null, new string('1', UserConfigurations.PhoneMaxLength + 1),
            null, false, false, false, true));

        Assert.That(longFirstName.ResultType, Is.EqualTo(ResultType.Invalid));
        Assert.That(longLastName.ResultType, Is.EqualTo(ResultType.Invalid));
        Assert.That(longPhoto.ResultType, Is.EqualTo(ResultType.Invalid));
        Assert.That(longPhone.ResultType, Is.EqualTo(ResultType.Invalid));
    }

    [Test]
    public async Task ExecuteAsync_RejectsMissingEmailOrPhoneWhenSpecified()
    {
        (User Entity, long Id) user = await SeedUserAsync();
        var handler = new PatchUserCommandHandler(Users, UnitOfWork);
        Result<Guid> missingEmail = await handler.ExecuteAsync(
            new(user.Entity.Uid, " ", null, null, null, null, true, false, false, false));
        Result<Guid> missingPhone = await handler.ExecuteAsync(
            new(user.Entity.Uid, null, null, null, null, null, false, false, false, true));

        Assert.That(missingEmail.ResultType, Is.EqualTo(ResultType.Invalid));
        Assert.That(missingPhone.ResultType, Is.EqualTo(ResultType.Invalid));
    }

    [Test]
    public async Task ExecuteAsync_RejectsEmailAlreadyInUse()
    {
        await SeedUserAsync("existing@example.com");
        (User Entity, long Id) user = await SeedUserAsync("current@example.com");

        Result<Guid> result = await new PatchUserCommandHandler(Users, UnitOfWork).ExecuteAsync(
            new(user.Entity.Uid, "existing@example.com", null, null, null, null, true, false, false, false));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.Conflicted));
    }

    [Test]
    public async Task ExecuteAsync_RejectsEmailBeyondConfiguredLength()
    {
        (User Entity, long Id) user = await SeedUserAsync();
        string longDomain = string.Join('.', Enumerable.Repeat(new string('b', 63), 3)) + ".com";
        string email = $"{new string('a', 64)}@{longDomain}";

        Result<Guid> result = await new PatchUserCommandHandler(Users, UnitOfWork).ExecuteAsync(
            new(user.Entity.Uid, email, null, null, null, null, true, false, false, false));

        Assert.That(result.Message, Is.EqualTo(IdentityErrorMessages.EmailTooLong));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownUser()
    {
        Result<Guid> result = await new PatchUserCommandHandler(Users, UnitOfWork).ExecuteAsync(
            new(Guid.NewGuid(), null, "Name", null, null, null, false, true, false, false));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }
}
