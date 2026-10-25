using System.ComponentModel.DataAnnotations;
using App.Core.SharedLibrary.Errors;

namespace App.API.Controllers.Identity.Contracts;

public sealed class UserClaimRequest
{
    [Range(1, long.MaxValue, ErrorMessage = IdentityErrorMessages.UserIdMustBePositive)]
    public long UserId { get; init; }

    [Range(1, long.MaxValue, ErrorMessage = IdentityErrorMessages.ClaimTypeIdMustBePositive)]
    public long ClaimTypeId { get; init; }

    [Required(ErrorMessage = IdentityErrorMessages.ClaimValueRequired)]
    [StringLength(255, ErrorMessage = IdentityErrorMessages.ClaimValueTooLong)]
    public string ClaimValue { get; init; } = string.Empty;
}
