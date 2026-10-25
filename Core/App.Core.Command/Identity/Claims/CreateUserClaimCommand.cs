using App.Core.Domain.Identity.Claims;
using App.Core.Persistence.Configurations.Database.Context.UnitOfWork;
using App.Core.Persistence.Identity.Claims.Repositories;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;
using App.Core.SharedLibrary.Errors;

namespace App.Core.Command.Identity.Claims;

public sealed record CreateUserClaimCommand(long UserId, long ClaimTypeId, string ClaimValue) : ICommand<Result<Guid>>;

public sealed class CreateUserClaimCommandHandler(IUserClaimRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<CreateUserClaimCommand, Result<Guid>>
{
    public async Task<Result<Guid>> ExecuteAsync(CreateUserClaimCommand command, CancellationToken cancellationToken = default)
    {
        UserClaim entity = UserClaim.Create(command.UserId, command.ClaimTypeId, command.ClaimValue.Trim());
        Result inserted = await repository.InsertAsync(entity);
        if (inserted.IsFailure) return Result.FromError<Guid>(inserted);
        Result saved = await unitOfWork.SaveChangesAsync();
        return saved.IsSuccess ? Result.Ok(entity.Uid) : Result.FromError<Guid>(saved);
    }
    private static bool Valid(string value) => !string.IsNullOrWhiteSpace(value) && value.Trim().Length <= 255;
}
