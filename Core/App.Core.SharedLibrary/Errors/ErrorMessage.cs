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
    public const string NameRequired = "Name is required.";
    public const string EmailRequired = "Email is required.";
    public const string EmailAddressInvalid = "Enter a valid email address.";
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
    public const string ClaimNameRequired = "Claim name is required.";
    public const string ClaimNameTooLong = "Claim name cannot exceed 100 characters.";
    public const string ClaimDescriptionTooLong = "Claim description cannot exceed 255 characters.";
    public const string ClaimDataTypeInvalid = "Claim data type is invalid.";
    public const string ClaimModuleRequired = "Claim module is required.";
    public const string ClaimModuleTooLong = "Claim module cannot exceed 50 characters.";
    public const string RoleNameRequired = "Role name is required.";
    public const string RoleNameTooLong = "Role name cannot exceed 50 characters.";
    public const string RoleSlugTooLong = "Role slug cannot exceed 50 characters.";
    public const string RoleDescriptionTooLong = "Role description cannot exceed 255 characters.";
    public const string UserIdMustBePositive = "User ID must be positive.";
    public const string RoleIdMustBePositive = "Role ID must be positive.";
    public const string ClaimTypeIdMustBePositive = "Claim type ID must be positive.";
    public const string AssignedByMustBePositive = "AssignedBy must be a positive user ID.";
    public const string ClaimValueRequired = "Claim value is required.";
    public const string ClaimValueTooLong = "Claim value cannot exceed 255 characters.";
    public const string AssignmentCannotBeChanged = "Assignment identifiers cannot be changed; delete and recreate the assignment.";
    public const string ClaimTypeDataInvalid = "Claim name, module, data type, or description is invalid.";
    public const string RoleNameLengthInvalid = "Role name must be 1 to 50 characters.";
    public const string SystemRoleCannotBeDeleted = "System roles cannot be deleted.";
    public const string SystemRoleCannotBeBlocked = "System roles cannot be blocked.";
    public const string SystemRoleCannotBeChanged = "System roles cannot be renamed or changed here.";
}

public sealed class PersistenceErrorMessages : ErrorMessage
{
    public const string EntityNotFound = "EntityNotFoundError";
    public const string DatabaseInternalError = "DbInternalError";
}
