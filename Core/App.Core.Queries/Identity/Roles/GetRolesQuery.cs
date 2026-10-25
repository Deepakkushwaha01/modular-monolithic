using App.Core.Domain.Identity.Roles;
using App.Core.Persistence.Identity.Roles.Repositories;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;

namespace App.Core.Queries.Identity.Roles;

public sealed record GetRolesQuery(bool IncludeUsers = false, bool IncludeClaims = false) : IQuery<List<Role>>;

public sealed class GetRolesQueryHandler(IRoleRepository repository) : IQueryHandler<GetRolesQuery, List<Role>>
{
    public async Task<Result<List<Role>>> ExecuteAsync(GetRolesQuery query, CancellationToken cancellationToken = default)
    {
        Result<List<Role>> result = await repository.GetRolesAsync(query.IncludeUsers, query.IncludeClaims);
        return result.IsFailure ? Result.FromError<List<Role>>(result) : Result.Ok(result.Value);
    }
}
