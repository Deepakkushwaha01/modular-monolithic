using App.Core.Domain.Identity.Roles;
using App.Core.Persistence.Identity.Roles.Repositories;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;

namespace App.Core.Queries.Identity.Roles;

public sealed record GetRoleByUidQuery(Guid Uid, bool IncludeUsers = false, bool IncludeClaims = false) : IQuery<Role>;

public sealed class GetRoleByUidQueryHandler(IRoleRepository repository) : IQueryHandler<GetRoleByUidQuery, Role>
{
    public async Task<Result<Role>> ExecuteAsync(GetRoleByUidQuery query, CancellationToken cancellationToken = default)
    {
        Result<Role> result = await repository.GetRoleByUidAsync(query.Uid, query.IncludeUsers, query.IncludeClaims);
        return result.IsFailure ? Result.FromError<Role>(result) : Result.Ok(result.Value);
    }
}
