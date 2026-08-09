using App.Core.Command.Identity;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<IActionResult> CreateUser(CreateUserCommand command)
    {
        Result<Guid> result = await _commandDispatcher.ExecuteAsync(command);
        if (!result.IsSuccess)
            return StatusCode((int)result.HttpStatusCode, result.Message);

        return CreatedAtAction(nameof(GetUser), new { id = result.Value }, new { id = result.Value });
    }

    [HttpGet("users/{id:guid}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        Result<UserDto> result = await _queryDispatcher.ExecuteAsync(new GetUserQuery { Id = id });
        if (!result.IsSuccess)
            return StatusCode((int)result.HttpStatusCode, result.Message);

        return Ok(result.Value);
    }
}
