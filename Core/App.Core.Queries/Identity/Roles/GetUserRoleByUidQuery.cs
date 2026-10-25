using App.Core.Domain.Identity.Roles;
using App.Core.Persistence.Identity.Roles.Repositories;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;

namespace App.Core.Queries.Identity.Roles;

public sealed record GetUserRoleByUidQuery(Guid Uid) : IQuery<UserRole>;

public sealed class GetUserRoleByUidQueryHandler(IUserRoleRepository repository) : IQueryHandler<GetUserRoleByUidQuery, UserRole>
{
    public async Task<Result<UserRole>> ExecuteAsync(GetUserRoleByUidQuery query, CancellationToken cancellationToken = default)
    {
        Result<UserRole> result = await repository.GetUserRoleByUidAsync(query.Uid);
        return result.IsFailure ? Result.FromError<UserRole>(result) : Result.Ok(result.Value);
    }
}
