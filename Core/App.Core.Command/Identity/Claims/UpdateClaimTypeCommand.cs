using App.Core.Domain.Identity.Claims;
using App.Core.Persistence.Configurations.Database.Context.UnitOfWork;
using App.Core.Persistence.Identity.Claims.Repositories;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;
using App.Core.SharedLibrary.Errors;

namespace App.Core.Command.Identity.Claims;

public sealed record UpdateClaimTypeCommand(
    Guid Uid,
    string Name,
    string? Description,
    ClaimDataType DataType,
    string Module,
    bool IsActive
) : ICommand<Result<Guid>>;

public sealed class UpdateClaimTypeCommandHandler(
    IClaimTypeRepository repository,
    IUnitOfWork unitOfWork
) : ICommandHandler<UpdateClaimTypeCommand, Result<Guid>>
{
    public async Task<Result<Guid>> ExecuteAsync(
        UpdateClaimTypeCommand command,
        CancellationToken cancellationToken = default)
    {
        Result<ClaimType> found = await repository.GetClaimTypeByUidAsync(command.Uid);
        if (found.IsFailure)
        {
            return Result.FromError<Guid>(found);
        }

        found.Value.Update(
            command.Name.Trim(),
            CreateClaimTypeCommandHandler.Clean(command.Description),
            command.DataType,
            command.Module.Trim(),
            command.IsActive
        );

        repository.Update(found.Value);

        Result saved = await unitOfWork.SaveChangesAsync();
        return saved.IsSuccess
            ? Result.Ok(found.Value.Uid)
            : Result.FromError<Guid>(saved);
    }
}
