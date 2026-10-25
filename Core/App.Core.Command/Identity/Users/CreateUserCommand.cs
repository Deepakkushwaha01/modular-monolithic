using System.Net.Mail;
using App.Core.Domain.Identity.Users;
using App.Core.Persistence.Configurations.Database.Context.UnitOfWork;
using App.Core.Persistence.Identity.Users.Repositories;
using App.Core.SharedLibrary.Configuration.Identity;
using App.Core.SharedLibrary.Errors;
using App.Core.SharedLibrary.Helpers;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;
using Microsoft.AspNetCore.Identity;

namespace App.Core.Command.Identity.Users;

public class CreateUserCommand : ICommand<Result<Guid>>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Phone { get; set; }

    public CreateUserCommand(
        string email,
        string password,
        string phone,
        string? firstName = null,
        string? lastName = null)
    {
        Email = email;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
    }
}


public sealed class CreateUserCommandHandler(
    IUserRepository _userRepository,
    IUnitOfWork _unitOfWork,
    IPasswordHasher<User> passwordHasher)
    : ICommandHandler<CreateUserCommand, Result<Guid>>
{
    public async Task<Result<Guid>> ExecuteAsync(
        CreateUserCommand command,
        CancellationToken cancellationToken = default)
    {
        string email = command.Email?.Trim().ToLowerInvariant() ?? string.Empty;
        string password = command.Password ?? string.Empty;
        string rawPhone = command.Phone?.Trim() ?? string.Empty;

        if (!MailAddress.TryCreate(email, out MailAddress? address) ||
            !string.Equals(address.Address, email, StringComparison.OrdinalIgnoreCase))
        {
            return Result.Invalid<Guid>(IdentityErrorMessages.EmailInvalid);
        }

        if (password.Length < UserConfigurations.PasswordMinimumLength ||
            password.Length > UserConfigurations.PasswordMaximumLength)
        {
            return Result.Invalid<Guid>(string.Format(
                System.Globalization.CultureInfo.CurrentCulture,
                IdentityErrorMessages.PasswordLength,
                UserConfigurations.PasswordMinimumLength,
                UserConfigurations.PasswordMaximumLength));
        }

        if (!PhoneNumberHelper.TryNormalizeToE164(rawPhone, UserConfigurations.DefaultRegion, out string normalizedPhone))
            return Result.Invalid<Guid>(string.IsNullOrWhiteSpace(rawPhone)
                ? IdentityErrorMessages.PhoneRequired
                : IdentityErrorMessages.PhoneInvalidFormat);


        if (await _userRepository.EmailExistsAsync(email))
        {
            return Result.Conflicted<Guid>(IdentityErrorMessages.AccountEmailExists);
        }

        User passwordHashContext = User.Create(email, string.Empty, normalizedPhone);
        string passwordHash = passwordHasher.HashPassword(passwordHashContext, password);
        User user = User.Create(email, passwordHash, normalizedPhone, command.FirstName?.Trim(), command.LastName?.Trim());

        Result inserted = await _userRepository.InsertAsync(user);
        if (inserted.IsFailure)
        {
            return Result.FromError<Guid>(inserted);
        }

        Result saved = await _unitOfWork.SaveChangesAsync();
        return saved.IsSuccess
            ? Result.Ok(user.Uid)
            : Result.FromError<Guid>(saved);
    }
}
