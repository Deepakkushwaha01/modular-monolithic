using App.Core.Domain.Identity.Addresses;
using App.Core.Domain.Identity.Claims;
using App.Core.Domain.Identity.Roles;
using App.Core.Domain.Identity.Users;
using System.Text.Json.Serialization;

namespace App.API.Controllers.Identity.Contracts;

public sealed record RoleClaimResponse(Guid Uid, long RoleId, long ClaimTypeId, string ClaimValue, DateTime CreatedAt)
{
    public static RoleClaimResponse From(RoleClaim item) => new(item.Uid, item.RoleId, item.ClaimTypeId, item.ClaimValue, item.CreatedAt);
}
