using App.Core.Persistence.Configurations.Database.Context.UnitOfWork;
using App.Core.Persistence.Identity.Authentication.Factories;
using App.Core.Persistence.Identity.Authentication.Repositories;
using App.Core.Persistence.Identity.Users.Repositories;
using App.Core.SharedLibrary.Errors;
using App.Core.SharedLibrary.Helpers;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;
using DomainUser = App.Core.Domain.Identity.Users.User;
using PersistenceRefreshToken = App.Core.Persistence.Identity.Authentication.Entities.RefreshToken;

namespace App.Core.Command.Identity.Authentication;

public sealed record RefreshTokenCommand(string RefreshToken) : ICommand<Result<DomainUser>>;

public sealed class RefreshTokenCommandHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    JwtTokenHelper tokenHelper)
    : ICommandHandler<RefreshTokenCommand, Result<DomainUser>>
{
    public async Task<Result<DomainUser>> ExecuteAsync(
        RefreshTokenCommand command,
        CancellationToken cancellationToken = default)
    {
        string suppliedHash = tokenHelper.GetRefreshTokenHash(command.RefreshToken);
        PersistenceRefreshToken? storedToken = await refreshTokenRepository.GetByTokenHashAsync(suppliedHash);
        if (storedToken is null)
            return Result.Unauthorized<DomainUser>(IdentityErrorMessages.InvalidRefreshToken);

        // A previously rotated token being presented again indicates token theft. Revoke
        // every still-active token in that login session's family.
        if (storedToken.RevokedOn is not null)
        {
            await refreshTokenRepository.RevokeFamilyAsync(storedToken.FamilyUid);
            await unitOfWork.SaveChangesAsync();
            return Result.Unauthorized<DomainUser>(IdentityErrorMessages.InvalidRefreshToken);
        }

        if (!tokenHelper.TryValidateRefreshToken(command.RefreshToken, storedToken.TokenHash,
                new DateTimeOffset(DateTime.SpecifyKind(storedToken.ExpiresOn, DateTimeKind.Utc)), isRevoked: false))
        {
            bool revoked = await refreshTokenRepository.TryRevokeAsync(storedToken.Uid, null);
            if (revoked)
                await unitOfWork.SaveChangesAsync();
            return Result.Unauthorized<DomainUser>(IdentityErrorMessages.InvalidRefreshToken);
        }

        Result<DomainUser> userResult = await userRepository.GetUserWithRolesAndClaimsByUidAsync(storedToken.UserUid);
        if (userResult.IsFailure)
        {
            await refreshTokenRepository.RevokeFamilyAsync(storedToken.FamilyUid);
            await unitOfWork.SaveChangesAsync();
            return Result.Unauthorized<DomainUser>(IdentityErrorMessages.InvalidRefreshToken);
        }

        DomainUser user = userResult.Value;
        RefreshTokenResult replacement = tokenHelper.CreateRefreshToken();
        bool consumed = await refreshTokenRepository.TryRevokeAsync(storedToken.Uid, replacement.TokenHash);
        if (!consumed)
            return Result.Unauthorized<DomainUser>(IdentityErrorMessages.InvalidRefreshToken);

        await refreshTokenRepository.InsertAsync(App.Core.Domain.Identity.Authentication.RefreshToken
            .Create(user.Uid, storedToken.FamilyUid, replacement.TokenHash, replacement.ExpiresAt.UtcDateTime)
            .ToRefreshTokenEntity());

        string accessToken = tokenHelper.CreateAccessToken(user.Uid, user.Email, user.Portal, user.Roles, user.Claims);
        Result saved = await unitOfWork.SaveChangesAsync();
        if (saved.IsFailure)
            return Result.FromError<DomainUser>(saved);

        user.UpdateTokenPair(accessToken, replacement.Token, tokenHelper.GetAccessTokenExpiresAt(), replacement.ExpiresAt);
        return Result.Ok(user);
    }
}
