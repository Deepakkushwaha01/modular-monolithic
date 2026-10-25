using App.Core.Domain.Identity.Roles;
using App.Core.Persistence.Identity.Roles.Repositories;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;

namespace App.Core.Queries.Identity.Roles;

public sealed record GetUserRolesQuery : IQuery<List<UserRole>>;

public sealed class GetUserRolesQueryHandler(IUserRoleRepository repository) : IQueryHandler<GetUserRolesQuery, List<UserRole>>
{
    public async Task<Result<List<UserRole>>> ExecuteAsync(GetUserRolesQuery query, CancellationToken cancellationToken = default)
    {
        Result<List<UserRole>> result = await repository.GetUserRolesAsync();
        return result.IsFailure ? Result.FromError<List<UserRole>>(result) : Result.Ok(result.Value);
    }
}
