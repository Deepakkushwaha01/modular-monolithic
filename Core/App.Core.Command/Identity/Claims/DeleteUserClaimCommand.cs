using App.Core.Domain.Identity.Claims;
using App.Core.Persistence.Configurations.Database.Context.UnitOfWork;
using App.Core.Persistence.Identity.Claims.Repositories;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;

namespace App.Core.Command.Identity.Claims;

public sealed record DeleteUserClaimCommand(Guid Uid) : ICommand<Result>;

public sealed class DeleteUserClaimCommandHandler(IUserClaimRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<DeleteUserClaimCommand, Result>
{
    public async Task<Result> ExecuteAsync(DeleteUserClaimCommand command, CancellationToken cancellationToken = default)
    {
        Result deleted = await repository.DeleteByUidAsync(command.Uid);
        return deleted.IsFailure ? deleted : await unitOfWork.SaveChangesAsync();
    }
}
