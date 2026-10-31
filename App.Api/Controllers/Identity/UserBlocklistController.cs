using App.API.Settings;
using App.Core.Command.Identity.Users;
using App.Core.SharedLibrary.Errors;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers.Identity;

[ApiController, ApiVersion(1.0), SwaggerModule(SwaggerDocuments.Identity)]
[Authorize(Roles = "super-admin,admin")]
[Route("api/v{version:apiVersion}/users")]
public sealed class UserBlocklistController(
    ICommandDispatcher commands) : ControllerBase
{
    [HttpPost("{uid:guid}/block")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Block(Guid uid)
    {
        Result result = await commands.ExecuteAsync(new SetUserBlockedCommand(uid, true));
        return result.IsFailure ? Fail(result) : NoContent();
    }

    [HttpPost("{uid:guid}/unblock")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Unblock(Guid uid)
    {
        Result result = await commands.ExecuteAsync(new SetUserBlockedCommand(uid, false));
        return result.IsFailure ? Fail(result) : NoContent();
    }

    private IActionResult Fail(ResultCommonLogic result) =>
        StatusCode((int)result.HttpStatusCode, result.Message);
}
