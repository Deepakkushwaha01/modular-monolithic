namespace App.Core.SharedLibrary.Configuration.Identity;

public sealed class UserConfigurations
{
    public const int PasswordMinimumLength = 8;
    public const int PasswordMaximumLength = 128;
    public const int FirstNameMaximumLength = 100;
    public const int EmailMaximumLength = 256;
    public const int LastNameMaximumLength = 100;
    public const int ProfilePhotoMaximumLength = 2048;
    public const string DefaultRegion = "IN";
    public const string PhoneRegex = @"^\+?[1-9]\d{1,14}$";
    public const int PhoneMaxLength = 50;
    public const int PhoneStorageMaxLength = 20;
}
