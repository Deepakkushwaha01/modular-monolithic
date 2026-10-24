using System.ComponentModel.DataAnnotations;
using App.Core.SharedLibrary.Errors;
using App.Core.SharedLibrary.Configuration.Identity;

namespace App.API.Controllers.Identity.Contracts;

public sealed class CreateUserRequest
{
    [Required(ErrorMessage = IdentityErrorMessages.EmailRequired)]
    [EmailAddress(ErrorMessage = IdentityErrorMessages.EmailInvalid)]
    [MaxLength(UserConfigurations.EmailMaximumLength, ErrorMessage = IdentityErrorMessages.EmailTooLong)]
    public string Email { get; init; } = string.Empty;

    [Required(ErrorMessage = IdentityErrorMessages.PasswordRequired)]
    [MaxLength(UserConfigurations.PasswordMaximumLength, ErrorMessage = IdentityErrorMessages.PasswordTooLong)]
    [MinLength(UserConfigurations.PasswordMinimumLength, ErrorMessage = IdentityErrorMessages.PasswordTooShort)]
    public string Password { get; init; } = string.Empty;

    [MaxLength(UserConfigurations.FirstNameMaximumLength, ErrorMessage = IdentityErrorMessages.FirstNameTooLong)]
    public string? FirstName { get; init; }

    [MaxLength(UserConfigurations.LastNameMaximumLength, ErrorMessage = IdentityErrorMessages.LastNameTooLong)]
    public string? LastName { get; init; }

    [Required(ErrorMessage = IdentityErrorMessages.PhoneRequired)]
    [MaxLength(UserConfigurations.PhoneMaxLength, ErrorMessage = IdentityErrorMessages.PhoneTooLong)]
    [RegularExpression(UserConfigurations.PhoneRegex, ErrorMessage = IdentityErrorMessages.PhoneInvalidFormat)]
    public string Phone { get; init; } = string.Empty;
}
