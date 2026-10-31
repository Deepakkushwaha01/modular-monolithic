using System.ComponentModel.DataAnnotations;
using App.Core.SharedLibrary.Errors;

namespace App.API.Controllers.Identity.Contracts;

public sealed class UserClaimRequest
{
    [Required]
    public Guid? UserId { get; init; }

    [Required]
    public Guid? ClaimTypeId { get; init; }

    [Required(ErrorMessage = IdentityErrorMessages.ClaimValueRequired)]
    [StringLength(255, ErrorMessage = IdentityErrorMessages.ClaimValueTooLong)]
    public string ClaimValue { get; init; } = string.Empty;
}
