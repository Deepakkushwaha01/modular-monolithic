using System.ComponentModel.DataAnnotations;
using App.Core.SharedLibrary.Errors;

namespace App.API.Controllers.Identity.Contracts;

public sealed class RoleRequest
{
    [Required(ErrorMessage = IdentityErrorMessages.RoleNameRequired)]
    [StringLength(50, ErrorMessage = IdentityErrorMessages.RoleNameTooLong)]
    public string Name { get; init; } = string.Empty;

    [StringLength(50, ErrorMessage = IdentityErrorMessages.RoleSlugTooLong)]
    public string? Slug { get; init; }

    [StringLength(255, ErrorMessage = IdentityErrorMessages.RoleDescriptionTooLong)]
    public string? Description { get; init; }

    public bool IsSystemRole { get; init; }

    public bool IsActive { get; init; } = true;
}
