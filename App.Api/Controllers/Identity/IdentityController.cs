
using Asp.Versioning;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;
using App.Core.Command.Identity.Users;
using App.Core.Command.Identity.Users.Queries;
using App.Core.Domain.Identity.Users;
using App.API.Settings;
using App.API.Controllers.Identity.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[ApiController]
[ApiVersion(1.0)]
[SwaggerModule(SwaggerDocuments.Identity)]
[Route("api/v{version:apiVersion}/[controller]")]
public class IdentityController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public IdentityController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    [HttpPost("users")]
    [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        CreateUserCommand command = new(
            request.Email,
            request.Password,
            request.Phone,
            request.FirstName,
            request.LastName);

        Result<Guid> result = await _commandDispatcher.ExecuteAsync(command);
        if (!result.IsSuccess)
        {
            return StatusCode((int)result.HttpStatusCode, result.Message);
        }

        return CreatedAtAction(
            nameof(GetUser),
            new { uid = result.Value },
            new CreateUserResponse(result.Value, request.Email.Trim().ToLowerInvariant()));
    }

    [HttpGet("users/{uid:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(Guid uid)
    {
        Result<User> result = await _queryDispatcher.ExecuteAsync(new GetUserByUidQuery(uid));
        if (result.IsFailure)
            return StatusCode((int)result.HttpStatusCode, result.Message);

        return Ok(result.Value);
    }

    [HttpPut("users/{uid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    public async Task<IActionResult> UpdateUser(Guid uid, [FromBody] UpdateUserRequest request)
    {
        UpdateUserCommand command = new(
            uid,
            request.Email,
            request.Phone,
            request.FirstName,
            request.LastName);

        Result<Guid> result = await _commandDispatcher.ExecuteAsync(command);
        return result.IsSuccess
            ? NoContent()
            : StatusCode((int)result.HttpStatusCode, result.Message);
    }

    [HttpPatch("users/{uid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    public async Task<IActionResult> PatchUser(Guid uid, [FromBody] PatchUserRequest request)
    {
        PatchUserCommand command = new(
            uid,
            request.Email,
            request.FirstName,
            request.LastName,
            request.Phone,
            request.ProfilePhoto,
            request.EmailSpecified,
            request.FirstNameSpecified,
            request.LastNameSpecified,
            request.PhoneSpecified);

        Result<Guid> result = await _commandDispatcher.ExecuteAsync(command);
        return result.IsSuccess
            ? NoContent()
            : StatusCode((int)result.HttpStatusCode, result.Message);
    }

    [HttpDelete("users/{uid:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(Guid uid)
    {
        Result result = await _commandDispatcher.ExecuteAsync(new DeleteUserCommand(uid));
        return result.IsSuccess
            ? NoContent()
            : StatusCode((int)result.HttpStatusCode, result.Message);
    }
}
