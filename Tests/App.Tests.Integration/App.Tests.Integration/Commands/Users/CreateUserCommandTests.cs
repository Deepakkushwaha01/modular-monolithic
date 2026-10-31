using App.Core.Command.Identity.Users;
using App.Core.Domain.Identity.Users;
using App.Core.SharedLibrary.Errors;
using Microsoft.AspNetCore.Identity;

namespace App.Tests.Integration.Commands.Users;

public sealed class CreateUserCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_NormalizesInputAndPersistsPasswordHash()
    {
        CreateUserCommandHandler handler = new(Users, UnitOfWork, new PasswordHasher<User>());

        Result<Guid> result = await handler.ExecuteAsync(
            new("  PERSON@Example.com ", "StrongPass123", "+14155552671", " Test ", " User "));

        Assert.That(result.IsSuccess, Is.True);
        Result<User> saved = await Users.GetUserByUidAsync(result.Value);
        Assert.That(saved.Value.Email, Is.EqualTo("person@example.com"));
        Assert.That(saved.Value.FirstName, Is.EqualTo("Test"));
        Assert.That(saved.Value.LastName, Is.EqualTo("User"));
        Assert.That(saved.Value.Password, Is.Not.EqualTo("StrongPass123"));
        Assert.That(new PasswordHasher<User>().VerifyHashedPassword(saved.Value, saved.Value.Password, "StrongPass123"),
            Is.EqualTo(PasswordVerificationResult.Success));
    }

    [TestCase("not-an-email", "StrongPass123", "+14155552671")]
    [TestCase("person@example.com", "short", "+14155552671")]
    [TestCase("person@example.com", "StrongPass123", "invalid")]
    [TestCase("person@example.com", "StrongPass123", "")]
    public async Task ExecuteAsync_RejectsInvalidEmailPasswordOrPhone(string email, string password, string phone)
    {
        Result<Guid> result = await new CreateUserCommandHandler(Users, UnitOfWork, new PasswordHasher<User>())
            .ExecuteAsync(new(email, password, phone));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.Invalid));
    }

    [TestCase(7)]
    [TestCase(129)]
    public async Task ExecuteAsync_RejectsPasswordOutsideConfiguredLength(int length)
    {
        Result<Guid> result = await new CreateUserCommandHandler(Users, UnitOfWork, new PasswordHasher<User>())
            .ExecuteAsync(new("person@example.com", new string('a', length), "+14155552671"));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.Invalid));
    }

    [TestCase(8)]
    [TestCase(128)]
    public async Task ExecuteAsync_AcceptsPasswordAtConfiguredLengthBoundaries(int length)
    {
        Result<Guid> result = await new CreateUserCommandHandler(Users, UnitOfWork, new PasswordHasher<User>())
            .ExecuteAsync(new($"person{length}@example.com", new string('a', length), "+14155552671"));

        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public async Task ExecuteAsync_RejectsDuplicateEmail()
    {
        await SeedUserAsync("existing@example.com");

        Result<Guid> result = await new CreateUserCommandHandler(Users, UnitOfWork, new PasswordHasher<User>())
            .ExecuteAsync(new("EXISTING@example.com", "StrongPass123", "+14155552671"));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.Conflicted));
    }
}
