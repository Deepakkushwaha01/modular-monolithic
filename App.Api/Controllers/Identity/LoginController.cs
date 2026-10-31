using App.API.Controllers.Identity.Contracts;
using App.API.Settings;
using App.Core.Command.Identity.Authentication;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DomainUser = App.Core.Domain.Identity.Users.User;

namespace App.API.Controllers.Identity;

[ApiController]
[ApiVersion(1.0)]
[SwaggerModule(SwaggerDocuments.Identity)]
[Route("api/v{version:apiVersion}/identity/auth")]
public sealed class LoginController(
    ICommandDispatcher commandDispatcher) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        Result<DomainUser> result = await commandDispatcher.ExecuteAsync(
            new LoginCommand(request.Email, request.Password), cancellationToken);

        if (result.IsFailure)
        {
            return StatusCode((int)result.HttpStatusCode, result.Message);
        }

        return Ok(new LoginResponse(result.Value.AccessToken,
            result.Value.RefreshToken, result.Value.Uid));
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        Result<DomainUser> result = await commandDispatcher.ExecuteAsync(
            new RefreshTokenCommand(request.RefreshToken), cancellationToken);

        if (result.IsFailure)
            return StatusCode((int)result.HttpStatusCode, result.Message);

        return Ok(new LoginResponse(result.Value.AccessToken,
            result.Value.RefreshToken, result.Value.Uid));
    }
}
