using App.Core.Domain.Identity.Addresses;
using App.Core.Domain.Identity.Claims;
using App.Core.Domain.Identity.Roles;
using App.Core.Domain.Identity.Users;
using System.Text.Json.Serialization;

namespace App.API.Controllers.Identity.Contracts;

public sealed record RoleResponse(Guid Uid, string Name, string Slug, string? Description, bool IsSystemRole, bool IsActive,
    DateTime CreatedOn,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] IReadOnlyList<UserRoleResponse>? UserRoles = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] IReadOnlyList<RoleClaimResponse>? RoleClaims = null)
{
    public static RoleResponse From(Role role, bool includeUsers = false, bool includeClaims = false) => new(
        role.Uid, role.Name, role.Slug, role.Description, role.IsSystemRole, role.IsActive, role.CreatedOn,
        includeUsers ? role.UserRoles.Select(UserRoleResponse.From).ToList() : null,
        includeClaims ? role.RoleClaims.Select(RoleClaimResponse.From).ToList() : null);
}
