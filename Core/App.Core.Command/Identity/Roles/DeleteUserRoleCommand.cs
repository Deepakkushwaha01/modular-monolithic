using App.Core.Domain.Identity.Roles;
using App.Core.Persistence.Configurations.Database.Context.UnitOfWork;
using App.Core.Persistence.Identity.Roles.Repositories;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;

namespace App.Core.Command.Identity.Roles;

public sealed record DeleteUserRoleCommand(Guid Uid) : ICommand<Result>;

public sealed class DeleteUserRoleCommandHandler(IUserRoleRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<DeleteUserRoleCommand, Result>
{
    public async Task<Result> ExecuteAsync(DeleteUserRoleCommand command, CancellationToken cancellationToken = default)
    {
        Result deleted = await repository.DeleteByUidAsync(command.Uid);
        return deleted.IsFailure ? deleted : await unitOfWork.SaveChangesAsync();
    }
}
