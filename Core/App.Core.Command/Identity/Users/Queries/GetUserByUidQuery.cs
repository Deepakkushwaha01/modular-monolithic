using App.Core.Domain.Identity.Users;
using App.Core.Persistence.Identity.Users.Repositories;
using App.Core.SharedLibrary.Errors;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;

namespace App.Core.Command.Identity.Users.Queries;

public sealed record GetUserByUidQuery(Guid Uid) : IQuery<User>;

public sealed class GetUserByUidQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetUserByUidQuery, User>
{
    public async Task<Result<User>> ExecuteAsync(GetUserByUidQuery query, CancellationToken cancellationToken = default) 
    {
        User user = await userRepository.GetUserByUidAsync(query.Uid);
        if(user is null)
            return Result.NotFound<User>(IdentityErrorMessages.UserNotFound);
        
        return Result.Ok(user);
    }
}
