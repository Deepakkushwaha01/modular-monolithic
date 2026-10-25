using App.Core.Domain.Identity.Roles;
using App.Core.Persistence.Configurations.Database.Context.UnitOfWork;
using App.Core.Persistence.Identity.Roles.Repositories;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;
using App.Core.SharedLibrary.Errors;

namespace App.Core.Command.Identity.Roles;

public sealed record CreateRoleCommand(string Name, string? Slug, string? Description, bool IsSystemRole = false) : ICommand<Result<Guid>>;

public sealed class CreateRoleCommandHandler(IRoleRepository repository, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateRoleCommand, Result<Guid>>
{
    public async Task<Result<Guid>> ExecuteAsync(CreateRoleCommand command, CancellationToken cancellationToken = default)
    {
        string name = command.Name?.Trim() ?? string.Empty;
        string slug = NormalizeSlug(command.Slug, name);
        Role role = Role.Create(name, slug, Clean(command.Description), command.IsSystemRole);
        Result inserted = await repository.InsertAsync(role);
        if (inserted.IsFailure) return Result.FromError<Guid>(inserted);
        Result saved = await unitOfWork.SaveChangesAsync();
        return saved.IsSuccess ? Result.Ok(role.Uid) : Result.FromError<Guid>(saved);
    }

    internal static string NormalizeSlug(string? slug, string name) =>
        string.Join('-', (string.IsNullOrWhiteSpace(slug) ? name : slug).Trim().ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries));

    internal static string? Clean(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
