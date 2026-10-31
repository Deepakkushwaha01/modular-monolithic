using App.Core.Domain.Identity.Roles;
using App.Core.Persistence.Configurations.Database.Context.UnitOfWork;
using App.Core.Persistence.Identity.Roles.Repositories;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;
using App.Core.SharedLibrary.Errors;

namespace App.Core.Command.Identity.Roles;

public sealed record UpdateUserRoleCommand(Guid Uid, Guid? AssignedBy) : ICommand<Result<Guid>>;

public sealed class UpdateUserRoleCommandHandler(IUserRoleRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateUserRoleCommand, Result<Guid>>
{
    public async Task<Result<Guid>> ExecuteAsync(UpdateUserRoleCommand command, CancellationToken cancellationToken = default)
    {
        if (command.AssignedBy == Guid.Empty) return Result.Invalid<Guid>(IdentityErrorMessages.AssignedByUidRequired);
        Result<UserRole> found = await repository.GetUserRoleByUidAsync(command.Uid);
        if (found.IsFailure) return Result.FromError<Guid>(found);
        found.Value.UpdateAssignedBy(command.AssignedBy);
        repository.Update(found.Value);
        Result saved = await unitOfWork.SaveChangesAsync();
        return saved.IsSuccess ? Result.Ok(found.Value.Uid) : Result.FromError<Guid>(saved);
    }
}
