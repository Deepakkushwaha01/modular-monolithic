using System.ComponentModel.DataAnnotations;

namespace App.API.Controllers.Identity.Contracts;

public sealed class RefreshTokenRequest
{
    [Required]
    [StringLength(256, MinimumLength = 32)]
    public string RefreshToken { get; init; } = string.Empty;
}
