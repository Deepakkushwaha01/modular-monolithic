using System.Net.Mail;
using App.Core.Domain.Identity.Users;
using App.Core.Persistence.Configurations.Database.Context.UnitOfWork;
using App.Core.Persistence.Identity.Users.Repositories;
using App.Core.SharedLibrary.Configuration.Identity;
using App.Core.SharedLibrary.Errors;
using App.Core.SharedLibrary.Helpers;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;

namespace App.Core.Command.Identity.Users;

public sealed record PatchUserCommand(
    Guid Uid,
    string? Email,
    string? FirstName,
    string? LastName,
    string? Phone,
    string? ProfilePhoto,
    bool EmailSpecified,
    bool FirstNameSpecified,
    bool LastNameSpecified,
    bool PhoneSpecified) : ICommand<Result<Guid>>;

public sealed class PatchUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<PatchUserCommand, Result<Guid>>
{
    public async Task<Result<Guid>> ExecuteAsync(PatchUserCommand command, CancellationToken cancellationToken = default)
    {
        if (!command.EmailSpecified && !command.FirstNameSpecified && !command.LastNameSpecified &&
            !command.PhoneSpecified)
            return Result.Invalid<Guid>(IdentityErrorMessages.NoUserFieldsToUpdate);

        if (command.FirstName?.Length > UserConfigurations.FirstNameMaximumLength)
            return Result.Invalid<Guid>(IdentityErrorMessages.FirstNameTooLong);
        if (command.LastName?.Length > UserConfigurations.LastNameMaximumLength)
            return Result.Invalid<Guid>(IdentityErrorMessages.LastNameTooLong);
        if (command.ProfilePhoto?.Length > UserConfigurations.ProfilePhotoMaximumLength)
            return Result.Invalid<Guid>(IdentityErrorMessages.ProfilePhotoTooLong);
        if (command.Phone?.Length > UserConfigurations.PhoneMaxLength)
            return Result.Invalid<Guid>(IdentityErrorMessages.PhoneTooLong);

        Result<User> found = await userRepository.GetUserByUidAsync(command.Uid);
        if (found.IsFailure)
            return Result.FromError<Guid>(found);

        User user = found.Value;
        if (command.EmailSpecified)
        {
            if (string.IsNullOrWhiteSpace(command.Email))
                return Result.Invalid<Guid>(IdentityErrorMessages.EmailRequired);

            string email = command.Email.Trim().ToLowerInvariant();
            if (!MailAddress.TryCreate(email, out MailAddress? address) ||
                !string.Equals(address.Address, email, StringComparison.OrdinalIgnoreCase))
                return Result.Invalid<Guid>(IdentityErrorMessages.EmailInvalid);

            if (email.Length > UserConfigurations.EmailMaximumLength)
                return Result.Invalid<Guid>(IdentityErrorMessages.EmailTooLong);

            if (!string.Equals(user.Email, email, StringComparison.OrdinalIgnoreCase))
            {
                if (await userRepository.EmailExistsAsync(email))
                    return Result.Conflicted<Guid>(IdentityErrorMessages.AccountEmailExists);
                user.UpdateEmail(email);
            }
        }

        if (command.PhoneSpecified)
        {
            if (string.IsNullOrWhiteSpace(command.Phone))
                return Result.Invalid<Guid>(IdentityErrorMessages.PhoneRequired);
            if (!PhoneNumberHelper.TryNormalizeToE164(command.Phone, UserConfigurations.DefaultRegion, out string normalizedPhone))
                return Result.Invalid<Guid>(IdentityErrorMessages.PhoneInvalidFormat);

            user.UpdatePhone(normalizedPhone);
        }

        if (command.FirstNameSpecified || command.LastNameSpecified)
        {
            user.UpdateProfile(
                command.FirstNameSpecified ? UpdateUserCommandHandler.NormalizeOptional(command.FirstName) : user.FirstName,
                command.LastNameSpecified ? UpdateUserCommandHandler.NormalizeOptional(command.LastName) : user.LastName,
                user.Phone);
        }

        userRepository.Update(user);
        Result saved = await unitOfWork.SaveChangesAsync();
        return saved.IsSuccess ? Result.Ok(user.Uid) : Result.FromError<Guid>(saved);
    }
}
