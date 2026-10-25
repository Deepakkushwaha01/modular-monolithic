using App.Core.Domain.Identity.Claims;
using App.Core.Persistence.Identity.Claims.Repositories;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;

namespace App.Core.Queries.Identity.Claims;

public sealed record GetRoleClaimByUidQuery(Guid Uid) : IQuery<RoleClaim>;

public sealed class GetRoleClaimByUidQueryHandler(IRoleClaimRepository repository) : IQueryHandler<GetRoleClaimByUidQuery, RoleClaim>
{
    public async Task<Result<RoleClaim>> ExecuteAsync(GetRoleClaimByUidQuery query, CancellationToken cancellationToken = default)
    {
        Result<RoleClaim> result = await repository.GetRoleClaimByUidAsync(query.Uid);
        return result.IsFailure ? Result.FromError<RoleClaim>(result) : Result.Ok(result.Value);
    }
}
