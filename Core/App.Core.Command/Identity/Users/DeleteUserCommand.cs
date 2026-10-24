using App.Core.Persistence.Configurations.Database.Context.UnitOfWork;
using App.Core.Persistence.Identity.Users.Repositories;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;

namespace App.Core.Command.Identity.Users;

public sealed record DeleteUserCommand(Guid Uid) : ICommand<Result>;

public sealed class DeleteUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteUserCommand, Result>
{
    public async Task<Result> ExecuteAsync(DeleteUserCommand command, CancellationToken cancellationToken = default)
    {
        Result deleted = await userRepository.DeleteByUidAsync(command.Uid);
        if (deleted.IsFailure)
            return deleted;

        Result saved = await unitOfWork.SaveChangesAsync();
        return saved.IsSuccess ? Result.Ok() : saved;
    }
}
