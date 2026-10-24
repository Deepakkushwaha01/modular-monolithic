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

public sealed record UpdateUserCommand(
    Guid Uid,
    string Email,
    string Phone,
    string? FirstName,
    string? LastName) : ICommand<Result<Guid>>;

public sealed class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateUserCommand, Result<Guid>>
{
    public async Task<Result<Guid>> ExecuteAsync(UpdateUserCommand command, CancellationToken cancellationToken = default)
    {

        string email = command.Email.Trim().ToLowerInvariant();
        if (!MailAddress.TryCreate(email, out MailAddress? address) ||
            !string.Equals(address.Address, email, StringComparison.OrdinalIgnoreCase))
            return Result.Invalid<Guid>(IdentityErrorMessages.EmailInvalid);

        if (!PhoneNumberHelper.TryNormalizeToE164(command.Phone, UserConfigurations.DefaultRegion, out string normalizedPhone))
            return Result.Invalid<Guid>(string.IsNullOrWhiteSpace(command.Phone)
                ? IdentityErrorMessages.PhoneRequired
                : IdentityErrorMessages.PhoneInvalidFormat);

        Result<User> found = await userRepository.GetUserByUidAsync(command.Uid);
        if (found.IsFailure)
            return Result.FromError<Guid>(found);

        User user = found.Value;
        if (!string.Equals(user.Email, email, StringComparison.OrdinalIgnoreCase))
        {
            if (await userRepository.EmailExistsAsync(email))
                return Result.Conflicted<Guid>(IdentityErrorMessages.AccountEmailExists);
            user.UpdateEmail(email);
        }

        user.UpdateProfile(
            NormalizeOptional(command.FirstName),
            NormalizeOptional(command.LastName),
            normalizedPhone);

        userRepository.Update(user);
        Result saved = await unitOfWork.SaveChangesAsync();
        return saved.IsSuccess ? Result.Ok(user.Uid) : Result.FromError<Guid>(saved);
    }

    internal static string? NormalizeOptional(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
