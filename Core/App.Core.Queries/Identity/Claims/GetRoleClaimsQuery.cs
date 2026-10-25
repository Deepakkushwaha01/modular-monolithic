using App.Core.Domain.Identity.Claims;
using App.Core.Persistence.Identity.Claims.Repositories;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;

namespace App.Core.Queries.Identity.Claims;

public sealed record GetRoleClaimsQuery : IQuery<List<RoleClaim>>;

public sealed class GetRoleClaimsQueryHandler(IRoleClaimRepository repository) : IQueryHandler<GetRoleClaimsQuery, List<RoleClaim>>
{
    public async Task<Result<List<RoleClaim>>> ExecuteAsync(GetRoleClaimsQuery query, CancellationToken cancellationToken = default)
    {
        Result<List<RoleClaim>> result = await repository.GetRoleClaimsAsync();
        return result.IsFailure ? Result.FromError<List<RoleClaim>>(result) : Result.Ok(result.Value);
    }
}
