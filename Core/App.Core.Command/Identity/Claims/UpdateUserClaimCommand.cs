using App.Core.Domain.Identity.Claims;
using App.Core.Persistence.Configurations.Database.Context.UnitOfWork;
using App.Core.Persistence.Identity.Claims.Repositories;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;
using App.Core.SharedLibrary.Errors;

namespace App.Core.Command.Identity.Claims;

public sealed record UpdateUserClaimCommand(Guid Uid, string ClaimValue) : ICommand<Result<Guid>>;

public sealed class UpdateUserClaimCommandHandler(IUserClaimRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateUserClaimCommand, Result<Guid>>
{
    public async Task<Result<Guid>> ExecuteAsync(UpdateUserClaimCommand command, CancellationToken cancellationToken = default)
    {
        Result<UserClaim> found = await repository.GetUserClaimByUidAsync(command.Uid);
        if (found.IsFailure) return Result.FromError<Guid>(found);
        found.Value.Update(command.ClaimValue.Trim());
        repository.Update(found.Value);
        Result saved = await unitOfWork.SaveChangesAsync();
        return saved.IsSuccess ? Result.Ok(found.Value.Uid) : Result.FromError<Guid>(saved);
    }
}
