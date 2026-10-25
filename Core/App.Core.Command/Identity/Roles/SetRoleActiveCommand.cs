using App.Core.Domain.Identity.Roles;
using App.Core.Persistence.Configurations.Database.Context.UnitOfWork;
using App.Core.Persistence.Identity.Roles.Repositories;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;
using App.Core.SharedLibrary.Errors;

namespace App.Core.Command.Identity.Roles;

public sealed record SetRoleActiveCommand(Guid Uid, bool IsActive) : ICommand<Result>;

public sealed class SetRoleActiveCommandHandler(IRoleRepository repository, IUnitOfWork unitOfWork)
    : ICommandHandler<SetRoleActiveCommand, Result>
{
    public async Task<Result> ExecuteAsync(SetRoleActiveCommand command, CancellationToken cancellationToken = default)
    {
        Result<Role> found = await repository.GetRoleByUidAsync(command.Uid);
        if (found.IsFailure) return Result.FromError(found);
        if (found.Value.IsSystemRole && !command.IsActive) return Result.Conflicted(IdentityErrorMessages.SystemRoleCannotBeBlocked);
        if (command.IsActive) found.Value.Activate(); else found.Value.Deactivate();
        repository.Update(found.Value);
        return await unitOfWork.SaveChangesAsync();
    }
}
