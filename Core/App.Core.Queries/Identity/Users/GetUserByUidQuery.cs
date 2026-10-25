using App.Core.Domain.Identity.Users;
using App.Core.Persistence.Identity.Users.Repositories;
using App.Core.SharedLibrary.Errors;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;

namespace App.Core.Queries.Identity.Users;

public sealed record GetUserByUidQuery(
    Guid Uid,
    bool IncludeAddresses = false,
    bool IncludeRoles = false,
    bool IncludeClaims = false) : IQuery<User>;

public sealed class GetUserByUidQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetUserByUidQuery, User>
{
    public async Task<Result<User>> ExecuteAsync(GetUserByUidQuery query, CancellationToken cancellationToken = default)
    {
        Result<User> result = await userRepository.GetUserByUidAsync(
            query.Uid,
            query.IncludeAddresses,
            query.IncludeRoles,
            query.IncludeClaims);

        return result.IsFailure
            ? Result.FromError<User>(result)
            : Result.Ok(result.Value);
    }
}
