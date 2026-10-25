using System.ComponentModel.DataAnnotations;
using App.Core.SharedLibrary.Errors;

namespace App.API.Controllers.Identity.Contracts;

public sealed class UserRoleRequest
{
    [Range(1, long.MaxValue, ErrorMessage = IdentityErrorMessages.UserIdMustBePositive)]
    public long UserId { get; init; }

    [Range(1, long.MaxValue, ErrorMessage = IdentityErrorMessages.RoleIdMustBePositive)]
    public long RoleId { get; init; }

    [Range(1, long.MaxValue, ErrorMessage = IdentityErrorMessages.AssignedByMustBePositive)]
    public long? AssignedBy { get; init; }
}
