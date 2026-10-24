using PhoneNumbers;

namespace App.Core.SharedLibrary.Helpers;

public static class PhoneNumberHelper
{
    private static readonly PhoneNumberUtil PhoneUtil = PhoneNumberUtil.GetInstance();

    public static bool IsValidRegionCode(string? regionCode) =>
        !string.IsNullOrWhiteSpace(regionCode) &&
        PhoneUtil.GetSupportedRegions().Contains(regionCode.Trim().ToUpperInvariant());

    /// <summary>
    /// Validates a phone number and returns its canonical E.164 form, including the country calling code.
    /// </summary>
    public static bool TryNormalizeToE164(string? phoneNumber, string defaultRegion, out string normalizedPhoneNumber)
    {
        normalizedPhoneNumber = string.Empty;
        if (string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(defaultRegion))
            return false;

        try
        {
            PhoneNumber parsed = PhoneUtil.Parse(phoneNumber, defaultRegion.Trim().ToUpperInvariant());
            if (!PhoneUtil.IsValidNumber(parsed))
                return false;

            normalizedPhoneNumber = PhoneUtil.Format(parsed, PhoneNumberFormat.E164);
            return true;
        }
        catch (NumberParseException)
        {
            return false;
        }
    }
}
