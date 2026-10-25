using App.Core.Domain.Identity.Roles;
using App.Core.Persistence.Configurations.Database.Context.UnitOfWork;
using App.Core.Persistence.Identity.Roles.Repositories;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;
using App.Core.SharedLibrary.Errors;

namespace App.Core.Command.Identity.Roles;

public sealed record CreateUserRoleCommand(long UserId, long RoleId, long? AssignedBy) : ICommand<Result<Guid>>;

public sealed class CreateUserRoleCommandHandler(IUserRoleRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<CreateUserRoleCommand, Result<Guid>>
{
    public async Task<Result<Guid>> ExecuteAsync(CreateUserRoleCommand command, CancellationToken cancellationToken = default)
    {
        if (command.UserId <= 0) return Result.Invalid<Guid>(IdentityErrorMessages.UserIdMustBePositive);
        if (command.RoleId <= 0) return Result.Invalid<Guid>(IdentityErrorMessages.RoleIdMustBePositive);
        if (command.AssignedBy is <= 0) return Result.Invalid<Guid>(IdentityErrorMessages.AssignedByMustBePositive);
        UserRole entity = UserRole.Create(command.UserId, command.RoleId, command.AssignedBy);
        Result inserted = await repository.InsertAsync(entity);
        if (inserted.IsFailure) return Result.FromError<Guid>(inserted);
        Result saved = await unitOfWork.SaveChangesAsync();
        return saved.IsSuccess ? Result.Ok(entity.Uid) : Result.FromError<Guid>(saved);
    }
}
