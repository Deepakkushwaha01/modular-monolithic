using App.Core.Domain.Identity.Claims;
using App.Core.Persistence.Configurations.Database.Context.UnitOfWork;
using App.Core.Persistence.Identity.Claims.Repositories;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;
using App.Core.SharedLibrary.Errors;

namespace App.Core.Command.Identity.Claims;

public sealed record CreateClaimTypeCommand(
    string Name,
    string? Description,
    ClaimDataType DataType,
    string Module
) : ICommand<Result<Guid>>;

public sealed class CreateClaimTypeCommandHandler(
    IClaimTypeRepository repository,
    IUnitOfWork unitOfWork
) : ICommandHandler<CreateClaimTypeCommand, Result<Guid>>
{
    public async Task<Result<Guid>> ExecuteAsync(
        CreateClaimTypeCommand command,
        CancellationToken cancellationToken = default)
    {
        ClaimType claim = ClaimType.Create(
            command.Name.Trim(),
            Clean(command.Description),
            command.DataType,
            command.Module.Trim()
        );

        Result inserted = await repository.InsertAsync(claim);
        if (inserted.IsFailure)
        {
            return Result.FromError<Guid>(inserted);
        }

        Result saved = await unitOfWork.SaveChangesAsync();
        return saved.IsSuccess
            ? Result.Ok(claim.Uid)
            : Result.FromError<Guid>(saved);
    }

    internal static string? Clean(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
