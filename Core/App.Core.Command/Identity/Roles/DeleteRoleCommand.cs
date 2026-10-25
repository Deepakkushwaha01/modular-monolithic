using App.Core.Domain.Identity.Roles;
using App.Core.Persistence.Configurations.Database.Context.UnitOfWork;
using App.Core.Persistence.Identity.Roles.Repositories;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;
using App.Core.SharedLibrary.Errors;

namespace App.Core.Command.Identity.Roles;

public sealed record DeleteRoleCommand(Guid Uid) : ICommand<Result>;

public sealed class DeleteRoleCommandHandler(IRoleRepository repository, IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteRoleCommand, Result>
{
    public async Task<Result> ExecuteAsync(DeleteRoleCommand command, CancellationToken cancellationToken = default)
    {
        Result<Role> found = await repository.GetRoleByUidAsync(command.Uid);
        if (found.IsFailure) return Result.FromError(found);
        if (found.Value.IsSystemRole) return Result.Conflicted(IdentityErrorMessages.SystemRoleCannotBeDeleted);
        Result deleted = await repository.DeleteByUidAsync(command.Uid);
        return deleted.IsFailure ? deleted : await unitOfWork.SaveChangesAsync();
    }
}
