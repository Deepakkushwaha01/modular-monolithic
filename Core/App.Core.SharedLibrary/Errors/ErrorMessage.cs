namespace App.Core.SharedLibrary.Errors;

/// <summary>Base type for grouped, reusable application error messages.</summary>
public abstract class ErrorMessage
{
    protected ErrorMessage() { }
}

public sealed class CommonErrorMessages : ErrorMessage
{
    public const string FailureMessageRequired = "An error message is required for a failure.";
    public const string FailureTypeRequired = "An error type is required for a failure.";
    public const string SuccessCannotHaveMessage = "A successful result cannot have an error message.";
    public const string SuccessCannotHaveErrorType = "A successful result cannot have an error type.";
    public const string SwaggerDocumentRequired = "At least one Swagger document must be configured.";
    public const string HandlerNotFound = "No handler found for {0} type {1}.";
}

public sealed class IdentityErrorMessages : ErrorMessage
{
    public const string EmailRequired = "Email is required.";
    public const string UserNotFound = "User not found.";
    public const string EmailInvalid = "Enter a valid email address.";
    public const string EmailTooLong = "Email cannot exceed 256 characters.";
    public const string PasswordRequired = "Password is required.";
    public const string PasswordLength = "Password must be between {0} and {1} characters.";
    public const string PasswordTooShort = "Password must contain at least {1} characters.";
    public const string PasswordTooLong = "Password cannot exceed {1} characters.";
    public const string PasswordLengthConfigurationInvalid = "Password length settings must be positive and maximum length must be at least minimum length.";
    public const string FirstNameTooLong = "First name cannot exceed 100 characters.";
    public const string LastNameTooLong = "Last name cannot exceed 100 characters.";
    public const string PhoneTooLong = "Phone input cannot exceed 50 characters.";
    public const string PhoneRequired = "Phone number is required.";
    public const string PhoneInvalidFormat = "Invalid phone number format.";
    public const string PhoneRegionInvalid = "Identity:Phone:DefaultRegion must be a valid two-letter region code.";
    public const string AccountEmailExists = "An account with this email already exists.";
    public const string NoUserFieldsToUpdate = "At least one user field must be provided for update.";
    public const string ProfilePhotoTooLong = "Profile photo URL cannot exceed 2048 characters.";
}

public sealed class PersistenceErrorMessages : ErrorMessage
{
    public const string EntityNotFound = "EntityNotFoundError";
    public const string DatabaseInternalError = "DbInternalError";
}
