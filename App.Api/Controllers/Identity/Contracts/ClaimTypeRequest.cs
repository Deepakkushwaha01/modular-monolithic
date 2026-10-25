using System.ComponentModel.DataAnnotations;
using App.Core.Domain.Identity.Claims;
using App.Core.SharedLibrary.Errors;

namespace App.API.Controllers.Identity.Contracts;

public sealed class ClaimTypeRequest
{
    [Required(ErrorMessage = IdentityErrorMessages.ClaimNameRequired)]
    [StringLength(100, ErrorMessage = IdentityErrorMessages.ClaimNameTooLong)]
    public string Name { get; init; } = string.Empty;

    [StringLength(255, ErrorMessage = IdentityErrorMessages.ClaimDescriptionTooLong)]
    public string? Description { get; init; }

    [Required(ErrorMessage = IdentityErrorMessages.ClaimDataTypeInvalid)]
    [EnumDataType(typeof(ClaimDataType), ErrorMessage = IdentityErrorMessages.ClaimDataTypeInvalid)]
    public ClaimDataType? DataType { get; init; }

    [Required(ErrorMessage = IdentityErrorMessages.ClaimModuleRequired)]
    [StringLength(50, ErrorMessage = IdentityErrorMessages.ClaimModuleTooLong)]
    public string Module { get; init; } = string.Empty;

    public bool IsActive { get; init; } = true;
}
