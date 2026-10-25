using App.Core.Domain.Identity.Claims;
using App.Core.Persistence.Configurations.Database.Context.UnitOfWork;
using App.Core.Persistence.Identity.Claims.Repositories;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;

namespace App.Core.Command.Identity.Claims;

public sealed record DeleteClaimTypeCommand(Guid Uid) : ICommand<Result>;

public sealed class DeleteClaimTypeCommandHandler(IClaimTypeRepository repository, IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteClaimTypeCommand, Result>
{
    public async Task<Result> ExecuteAsync(DeleteClaimTypeCommand command, CancellationToken cancellationToken = default)
    {
        Result deleted = await repository.DeleteByUidAsync(command.Uid);
        return deleted.IsFailure ? deleted : await unitOfWork.SaveChangesAsync();
    }
}
