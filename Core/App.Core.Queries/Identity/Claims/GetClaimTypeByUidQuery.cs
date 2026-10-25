using App.Core.Domain.Identity.Claims;
using App.Core.Persistence.Identity.Claims.Repositories;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;

namespace App.Core.Queries.Identity.Claims;

public sealed record GetClaimTypeByUidQuery(Guid Uid, bool IncludeRoleClaims = false, bool IncludeUserClaims = false) : IQuery<ClaimType>;

public sealed class GetClaimTypeByUidQueryHandler(IClaimTypeRepository repository) : IQueryHandler<GetClaimTypeByUidQuery, ClaimType>
{
    public async Task<Result<ClaimType>> ExecuteAsync(GetClaimTypeByUidQuery query, CancellationToken cancellationToken = default)
    {
        Result<ClaimType> result = await repository.GetClaimTypeByUidAsync(query.Uid, query.IncludeRoleClaims, query.IncludeUserClaims);
        return result.IsFailure ? Result.FromError<ClaimType>(result) : Result.Ok(result.Value);
    }
}
