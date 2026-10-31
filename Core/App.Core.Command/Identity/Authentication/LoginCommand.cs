using App.Core.Persistence.Configurations.Database.Context.UnitOfWork;
using App.Core.Persistence.Identity.Authentication.Factories;
using App.Core.Persistence.Identity.Authentication.Repositories;
using App.Core.Persistence.Identity.Users.Repositories;
using App.Core.SharedLibrary.Errors;
using App.Core.SharedLibrary.Helpers;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;
using Microsoft.AspNetCore.Identity;
using DomainUser = App.Core.Domain.Identity.Users.User;

namespace App.Core.Command.Identity.Authentication;

public sealed record LoginCommand(string Email, string Password) : ICommand<Result<DomainUser>>;

public sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher<DomainUser> passwordHasher,
    JwtTokenHelper tokenHelper)
    : ICommandHandler<LoginCommand, Result<DomainUser>>
{
    public async Task<Result<DomainUser>> ExecuteAsync(
        LoginCommand command,
        CancellationToken cancellationToken = default)
    {
        Result<DomainUser> loginDataResult = await userRepository.GetUserWithRolesAndClaimsAsync(command.Email);

        if (loginDataResult.ResultType == ResultType.NotFound)
        {
            return Result.Unauthorized<DomainUser>(IdentityErrorMessages.InvalidEmailOrPassword);
        }

        if (loginDataResult.IsFailure)
        {
            return Result.FromError<DomainUser>(loginDataResult);
        }

        DomainUser user = loginDataResult.Value;

        DomainUser passwordContext = DomainUser.Create(
            user.Email,
            user.Password,
            user.Phone,
            user.Portal);

        PasswordVerificationResult verification = passwordHasher.VerifyHashedPassword(
            passwordContext,
            user.Password,
            command.Password);

        if (verification == PasswordVerificationResult.Failed)
        {
            return Result.Unauthorized<DomainUser>(IdentityErrorMessages.InvalidEmailOrPassword);
        }

        Result updated = await userRepository.RecordSuccessfulLoginAsync(user.Uid);
        if (updated.IsFailure)
        {
            return Result.FromError<DomainUser>(updated);
        }

        string accessToken = tokenHelper.CreateAccessToken(user.Uid, user.Email, user.Portal, user.Roles, user.Claims);
        RefreshTokenResult refreshToken = tokenHelper.CreateRefreshToken();
        Guid familyUid = Guid.NewGuid();
        await refreshTokenRepository.InsertAsync(App.Core.Domain.Identity.Authentication.RefreshToken
            .Create(user.Uid, familyUid, refreshToken.TokenHash, refreshToken.ExpiresAt.UtcDateTime)
            .ToRefreshTokenEntity());

        Result saved = await unitOfWork.SaveChangesAsync();
        if (saved.IsFailure)
        {
            return Result.FromError<DomainUser>(saved);
        }

        user.UpdateTokenPair(accessToken, refreshToken.Token, tokenHelper.GetAccessTokenExpiresAt(), refreshToken.ExpiresAt);

        return Result.Ok(user);
    }
}
