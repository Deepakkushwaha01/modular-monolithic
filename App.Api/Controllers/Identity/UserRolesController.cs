using Asp.Versioning;
using App.API.Settings;
using App.API.Controllers.Identity.Contracts;
using App.Core.Command.Identity.Roles;
using App.Core.Domain.Identity.Roles;
using App.Core.Queries.Identity.Roles;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;
using App.Core.SharedLibrary.Errors;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers.Identity;

[ApiController, ApiVersion(1.0), SwaggerModule(SwaggerDocuments.Identity)]
[Route("api/v{version:apiVersion}/user-roles")]
public sealed class UserRolesController(ICommandDispatcher commands, IQueryDispatcher queries) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        Result<List<UserRole>> result = await queries.ExecuteAsync(new GetUserRolesQuery());
        return result.IsFailure ? Fail(result) : Ok(result.Value.Select(UserRoleResponse.From));
    }

    [HttpGet("{uid:guid}")]
    public async Task<IActionResult> Get(Guid uid)
    {
        Result<UserRole> result = await queries.ExecuteAsync(new GetUserRoleByUidQuery(uid));
        return result.IsFailure ? Fail(result) : Ok(UserRoleResponse.From(result.Value));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserRoleRequest request)
    {
        Result<Guid> result = await commands.ExecuteAsync(new CreateUserRoleCommand(request.UserId, request.RoleId, request.AssignedBy));
        return result.IsFailure ? Fail(result) : CreatedAtAction(nameof(Get), new { uid = result.Value }, new { uid = result.Value });
    }

    [HttpPut("{uid:guid}")]
    public async Task<IActionResult> Update(Guid uid, [FromBody] UserRoleRequest request)
    {
        Result<UserRole> found = await queries.ExecuteAsync(new GetUserRoleByUidQuery(uid));
        if (found.IsFailure) return Fail(found);
        if (found.Value.UserId != request.UserId || found.Value.RoleId != request.RoleId)
            return BadRequest(IdentityErrorMessages.AssignmentCannotBeChanged);
        Result<Guid> result = await commands.ExecuteAsync(new UpdateUserRoleCommand(uid, request.AssignedBy));
        return result.IsFailure ? Fail(result) : NoContent();
    }

    [HttpDelete("{uid:guid}")]
    public async Task<IActionResult> Delete(Guid uid)
    {
        Result result = await commands.ExecuteAsync(new DeleteUserRoleCommand(uid));
        return result.IsFailure ? Fail(result) : NoContent();
    }

    private IActionResult Fail(ResultCommonLogic result) => StatusCode((int)result.HttpStatusCode, result.Message);
}
