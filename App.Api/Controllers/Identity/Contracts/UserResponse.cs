using App.Core.Domain.Identity.Users;
using System.Text.Json.Serialization;

namespace App.API.Controllers.Identity.Contracts;

public sealed record UserResponse(
    Guid Uid,
    string Email,
    string? FirstName,
    string? LastName,
    string Phone,
    bool IsPhoneVerified,
    bool IsEmailVerified,
    bool IsActive,
    string? ProfilePhoto,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] IReadOnlyList<AddressResponse>? Addresses = null)
{
    public static UserResponse From(User user, bool includeAddresses = false) => new(
        user.Uid,
        user.Email,
        user.FirstName,
        user.LastName,
        user.Phone,
        user.IsPhoneVerified,
        user.IsEmailVerified,
        user.IsActive,
        user.ProfilePhoto,
        includeAddresses ? user.Addresses.Select(AddressResponse.From).ToList() : null);
}
