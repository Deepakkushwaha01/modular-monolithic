using App.Core.Domain.Identity.Claims;
using App.Core.Persistence.Identity.Claims.Repositories;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;

namespace App.Core.Queries.Identity.Claims;

public sealed record GetClaimTypesQuery(
    bool IncludeRoleClaims = false,
    bool IncludeUserClaims = false) : IQuery<List<ClaimType>>;

public sealed class GetClaimTypesQueryHandler(IClaimTypeRepository repository)
    : IQueryHandler<GetClaimTypesQuery, List<ClaimType>>
{
    public async Task<Result<List<ClaimType>>> ExecuteAsync(
        GetClaimTypesQuery query,
        CancellationToken cancellationToken = default)
    {
        Result<List<ClaimType>> result = await repository.GetClaimTypesAsync(
            query.IncludeRoleClaims,
            query.IncludeUserClaims);

        return result.IsFailure
            ? Result.FromError<List<ClaimType>>(result)
            : Result.Ok(result.Value);
    }
}