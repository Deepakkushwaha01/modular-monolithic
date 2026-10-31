namespace App.API.Controllers.Identity.Contracts;

public sealed record LoginResponse(
    string AccessToken,
    string RefreshToken,
    Guid UserUid);
