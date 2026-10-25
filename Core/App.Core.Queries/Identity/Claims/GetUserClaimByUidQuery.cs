using App.Core.Domain.Identity.Claims;
using App.Core.Persistence.Identity.Claims.Repositories;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;

namespace App.Core.Queries.Identity.Claims;

public sealed record GetUserClaimByUidQuery(Guid Uid) : IQuery<UserClaim>;

public sealed class GetUserClaimByUidQueryHandler(IUserClaimRepository repository) : IQueryHandler<GetUserClaimByUidQuery, UserClaim>
{
    public async Task<Result<UserClaim>> ExecuteAsync(GetUserClaimByUidQuery query, CancellationToken cancellationToken = default)
    {
        Result<UserClaim> result = await repository.GetUserClaimByUidAsync(query.Uid);
        return result.IsFailure ? Result.FromError<UserClaim>(result) : Result.Ok(result.Value);
    }
}
