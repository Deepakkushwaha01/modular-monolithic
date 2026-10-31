using System.ComponentModel.DataAnnotations;

namespace App.API.Controllers.Identity.Contracts;

public sealed class LoginRequest
{
    [Required, EmailAddress, MaxLength(320)]
    public string Email { get; init; } = string.Empty;

    [Required, MinLength(1), MaxLength(256)]
    public string Password { get; init; } = string.Empty;
}
