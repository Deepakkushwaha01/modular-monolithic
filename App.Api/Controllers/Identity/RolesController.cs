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
[Route("api/v{version:apiVersion}/roles")]
public sealed class RolesController(ICommandDispatcher commands, IQueryDispatcher queries) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includeUsers = false, [FromQuery] bool includeClaims = false)
    {
        Result<List<Role>> result = await queries.ExecuteAsync(new GetRolesQuery(includeUsers, includeClaims));
        return result.IsFailure ? Fail(result) : Ok(result.Value.Select(x => RoleResponse.From(x, includeUsers, includeClaims)));
    }

    [HttpGet("{uid:guid}")]
    public async Task<IActionResult> Get(Guid uid, [FromQuery] bool includeUsers = false, [FromQuery] bool includeClaims = false)
    {
        Result<Role> result = await queries.ExecuteAsync(new GetRoleByUidQuery(uid, includeUsers, includeClaims));
        return result.IsFailure ? Fail(result) : Ok(RoleResponse.From(result.Value, includeUsers, includeClaims));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RoleRequest request)
    {
        Result<Guid> result = await commands.ExecuteAsync(new CreateRoleCommand(request.Name, request.Slug, request.Description, request.IsSystemRole));
        return result.IsFailure ? Fail(result) : CreatedAtAction(nameof(Get), new { uid = result.Value }, new { uid = result.Value });
    }

    [HttpPut("{uid:guid}")]
    public async Task<IActionResult> Update(Guid uid, [FromBody] RoleRequest request)
    {
        Result<Guid> result = await commands.ExecuteAsync(new UpdateRoleCommand(uid, request.Name, request.Slug, request.Description, request.IsActive));
        return result.IsFailure ? Fail(result) : NoContent();
    }

    [HttpPost("{uid:guid}/block")]
    public Task<IActionResult> Block(Guid uid) => SetActive(uid, false);

    [HttpPost("{uid:guid}/unblock")]
    public Task<IActionResult> Unblock(Guid uid) => SetActive(uid, true);

    private async Task<IActionResult> SetActive(Guid uid, bool active)
    {
        Result result = await commands.ExecuteAsync(new SetRoleActiveCommand(uid, active));
        return result.IsFailure ? Fail(result) : NoContent();
    }

    [HttpDelete("{uid:guid}")]
    public async Task<IActionResult> Delete(Guid uid)
    {
        Result result = await commands.ExecuteAsync(new DeleteRoleCommand(uid));
        return result.IsFailure ? Fail(result) : NoContent();
    }

    private IActionResult Fail(ResultCommonLogic result) => StatusCode((int)result.HttpStatusCode, result.Message);
}
