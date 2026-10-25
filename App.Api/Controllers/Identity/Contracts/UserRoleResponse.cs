using App.Core.Domain.Identity.Addresses;
using App.Core.Domain.Identity.Claims;
using App.Core.Domain.Identity.Roles;
using App.Core.Domain.Identity.Users;
using System.Text.Json.Serialization;

namespace App.API.Controllers.Identity.Contracts;

public sealed record UserRoleResponse(Guid Uid, long UserId, long RoleId, DateTime AssignedAt, long? AssignedBy)
{
    public static UserRoleResponse From(UserRole item) => new(item.Uid, item.UserId, item.RoleId, item.AssignedAt, item.AssignedBy);
}
