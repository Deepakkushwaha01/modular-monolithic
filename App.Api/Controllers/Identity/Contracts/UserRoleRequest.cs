using System.ComponentModel.DataAnnotations;

namespace App.API.Controllers.Identity.Contracts;

public sealed class UserRoleRequest
{
    [Required]
    public Guid? UserId { get; init; }

    [Required]
    public Guid? RoleId { get; init; }

    public Guid? AssignedBy { get; init; }
}
