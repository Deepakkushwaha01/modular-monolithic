using System.Text.Json.Serialization;
using App.Core.Domain.Identity.Addresses;
using App.Core.Domain.Identity.Claims;
using App.Core.Domain.Identity.Roles;
using App.Core.Domain.Identity.Users;

namespace App.API.Controllers.Identity.Contracts;

public sealed record ClaimTypeResponse(
    Guid Uid,
    string Name,
    string? Description,
    ClaimDataType DataType,
    string Module,
    bool IsActive,
    DateTime CreatedOn,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    IReadOnlyList<RoleClaimResponse>? RoleClaims = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    IReadOnlyList<UserClaimResponse>? UserClaims = null
)
{
    public static ClaimTypeResponse From(
        ClaimType item,
        bool includeRoleClaims = false,
        bool includeUserClaims = false) =>
        new(
            item.Uid,
            item.Name,
            item.Description,
            item.DataType,
            item.Module,
            item.IsActive,
            item.CreatedOn,
            includeRoleClaims ? item.RoleClaims.Select(RoleClaimResponse.From).ToList() : null,
            includeUserClaims ? item.UserClaims.Select(UserClaimResponse.From).ToList() : null
        );
}