using App.Core.Command.Identity.Users;
using App.Core.Domain.Identity.Users;
using App.Core.SharedLibrary.Errors;

namespace App.Tests.Integration.Commands.Users;

public sealed class DeleteUserCommandTests : IntegrationTestBase
{
    [Test]
    public async Task ExecuteAsync_DeletesExistingUser()
    {
        (User Entity, long Id) user = await SeedUserAsync();

        Result result = await new DeleteUserCommandHandler(Users, UnitOfWork)
            .ExecuteAsync(new(user.Entity.Uid));

        Assert.That(result.IsSuccess, Is.True);
        Assert.That((await Users.GetUserByUidAsync(user.Entity.Uid)).ResultType, Is.EqualTo(ResultType.NotFound));
    }

    [Test]
    public async Task ExecuteAsync_ReturnsNotFoundForUnknownUser()
    {
        Result result = await new DeleteUserCommandHandler(Users, UnitOfWork)
            .ExecuteAsync(new(Guid.NewGuid()));

        Assert.That(result.ResultType, Is.EqualTo(ResultType.NotFound));
    }
}
