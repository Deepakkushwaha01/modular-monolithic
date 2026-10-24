using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using App.Core.SharedLibrary.Errors;
using App.Core.SharedLibrary.Configuration.Identity;

namespace App.API.Controllers.Identity.Contracts;

public sealed class PatchUserRequest
{
    private string? _email;
    private string? _firstName;
    private string? _lastName;
    private string? _phone;
    private string? _profilePhoto;

    [EmailAddress(ErrorMessage = IdentityErrorMessages.EmailInvalid)]
    [MaxLength(UserConfigurations.EmailMaximumLength, ErrorMessage = IdentityErrorMessages.EmailTooLong)]
    public string? Email { get => _email; init { _email = value; EmailSpecified = true; } }

    [MaxLength(UserConfigurations.FirstNameMaximumLength, ErrorMessage = IdentityErrorMessages.FirstNameTooLong)]
    public string? FirstName { get => _firstName; init { _firstName = value; FirstNameSpecified = true; } }

    [MaxLength(UserConfigurations.LastNameMaximumLength, ErrorMessage = IdentityErrorMessages.LastNameTooLong)]
    public string? LastName { get => _lastName; init { _lastName = value; LastNameSpecified = true; } }

    [MaxLength(UserConfigurations.PhoneMaxLength, ErrorMessage = IdentityErrorMessages.PhoneTooLong)]
    [RegularExpression(UserConfigurations.PhoneRegex, ErrorMessage = IdentityErrorMessages.PhoneInvalidFormat)]
    public string? Phone { get => _phone; init { _phone = value; PhoneSpecified = true; } }

    [MaxLength(UserConfigurations.ProfilePhotoMaximumLength, ErrorMessage = IdentityErrorMessages.ProfilePhotoTooLong)]
    public string? ProfilePhoto { get => _profilePhoto; init { _profilePhoto = value; ProfilePhotoSpecified = true; } }

    [JsonIgnore]
    public bool EmailSpecified { get; private set; }
    [JsonIgnore]
    public bool FirstNameSpecified { get; private set; }
    [JsonIgnore]
    public bool LastNameSpecified { get; private set; }
    [JsonIgnore]
    public bool PhoneSpecified { get; private set; }
    [JsonIgnore]
    public bool ProfilePhotoSpecified { get; private set; }
}
