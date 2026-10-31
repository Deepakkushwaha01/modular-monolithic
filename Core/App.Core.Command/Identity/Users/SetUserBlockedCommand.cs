using App.Core.Domain.Identity.Users;
using App.Core.Persistence.Configurations.Database.Context.UnitOfWork;
using App.Core.Persistence.Identity.Users.Repositories;
using App.Core.SharedLibrary.Helpers;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;

namespace App.Core.Command.Identity.Users;

public sealed record SetUserBlockedCommand(Guid Uid, bool IsBlocked) : ICommand<Result>;

public sealed class SetUserBlockedCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    RedisCacheService redisCache)
    : ICommandHandler<SetUserBlockedCommand, Result>
{
    public async Task<Result> ExecuteAsync(
        SetUserBlockedCommand command,
        CancellationToken cancellationToken = default)
    {
        Result<User> found = await userRepository.GetUserByUidAsync(command.Uid);
        if (found.IsFailure)
        {
            return Result.FromError(found);
        }

        User user = found.Value;

        if (command.IsBlocked)
        {
            user.Deactivate();
        }
        else
        {
            user.Activate();
        }

        userRepository.Update(user);

        Result saved = await unitOfWork.SaveChangesAsync();
        if (saved.IsFailure)
        {
            return Result.FromError(saved);
        }

        if (command.IsBlocked)
        {
            await redisCache.AddToSetAsync(
                RedisCacheConstants.BlockedUsersKey,
                command.Uid.ToString("D"),
                TimeSpan.FromDays(1));
        }
        else
        {
            await redisCache.RemoveFromSetAsync(
                RedisCacheConstants.BlockedUsersKey,
                command.Uid.ToString("D"));
        }

        return saved;
    }
}
