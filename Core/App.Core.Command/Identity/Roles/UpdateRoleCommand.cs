using App.Core.Domain.Identity.Roles;
using App.Core.Persistence.Configurations.Database.Context.UnitOfWork;
using App.Core.Persistence.Identity.Roles.Repositories;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;
using App.Core.SharedLibrary.Errors;

namespace App.Core.Command.Identity.Roles;

public sealed record UpdateRoleCommand(Guid Uid, string Name, string? Slug, string? Description, bool IsActive) : ICommand<Result<Guid>>;

public sealed class UpdateRoleCommandHandler(IRoleRepository repository, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateRoleCommand, Result<Guid>>
{
    public async Task<Result<Guid>> ExecuteAsync(UpdateRoleCommand command, CancellationToken cancellationToken = default)
    {
        string name = command.Name?.Trim() ?? string.Empty;

        Result<Role> found = await repository.GetRoleByUidAsync(command.Uid);
        if (found.IsFailure) return Result.FromError<Guid>(found);
        if (found.Value.IsSystemRole) return Result.Conflicted<Guid>(IdentityErrorMessages.SystemRoleCannotBeChanged);
        found.Value.UpdateName(name, CreateRoleCommandHandler.NormalizeSlug(command.Slug, name));
        found.Value.UpdateDescription(CreateRoleCommandHandler.Clean(command.Description));
        if (command.IsActive) found.Value.Activate(); else found.Value.Deactivate();
        repository.Update(found.Value);
        Result saved = await unitOfWork.SaveChangesAsync();
        return saved.IsSuccess ? Result.Ok(found.Value.Uid) : Result.FromError<Guid>(saved);
    }
}
