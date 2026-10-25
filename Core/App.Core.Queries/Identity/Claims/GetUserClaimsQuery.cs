using App.Core.Domain.Identity.Claims;
using App.Core.Persistence.Identity.Claims.Repositories;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;

namespace App.Core.Queries.Identity.Claims;

public sealed record GetUserClaimsQuery : IQuery<List<UserClaim>>;

public sealed class GetUserClaimsQueryHandler(IUserClaimRepository repository) : IQueryHandler<GetUserClaimsQuery, List<UserClaim>>
{
    public async Task<Result<List<UserClaim>>> ExecuteAsync(GetUserClaimsQuery query, CancellationToken cancellationToken = default)
    {
        Result<List<UserClaim>> result = await repository.GetUserClaimsAsync();
        return result.IsFailure ? Result.FromError<List<UserClaim>>(result) : Result.Ok(result.Value);
    }
}
