using Asp.Versioning;
using App.API.Settings;
using App.API.Controllers.Identity.Contracts;
using App.Core.Command.Identity.Claims;
using App.Core.Domain.Identity.Claims;
using App.Core.Queries.Identity.Claims;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;
using App.Core.SharedLibrary.Errors;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers.Identity;

[ApiController, ApiVersion(1.0), SwaggerModule(SwaggerDocuments.Identity)]
[Route("api/v{version:apiVersion}/role-claims")]
public sealed class RoleClaimsController(ICommandDispatcher commands, IQueryDispatcher queries) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        Result<List<RoleClaim>> result = await queries.ExecuteAsync(new GetRoleClaimsQuery());
        return result.IsFailure ? Fail(result) : Ok(result.Value.Select(RoleClaimResponse.From));
    }

    [HttpGet("{uid:guid}")]
    public async Task<IActionResult> Get(Guid uid)
    {
        Result<RoleClaim> result = await queries.ExecuteAsync(new GetRoleClaimByUidQuery(uid));
        return result.IsFailure ? Fail(result) : Ok(RoleClaimResponse.From(result.Value));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RoleClaimRequest request)
    {
        Result<Guid> result = await commands.ExecuteAsync(new CreateRoleClaimCommand(request.RoleId, request.ClaimTypeId, request.ClaimValue));
        return result.IsFailure ? Fail(result) : CreatedAtAction(nameof(Get), new { uid = result.Value }, new { uid = result.Value });
    }

    [HttpPut("{uid:guid}")]
    public async Task<IActionResult> Update(Guid uid, [FromBody] RoleClaimRequest request)
    {
        Result<Guid> result = await commands.ExecuteAsync(new UpdateRoleClaimCommand(uid, request.ClaimValue));
        return result.IsFailure ? Fail(result) : NoContent();
    }

    [HttpDelete("{uid:guid}")]
    public async Task<IActionResult> Delete(Guid uid)
    {
        Result result = await commands.ExecuteAsync(new DeleteRoleClaimCommand(uid));
        return result.IsFailure ? Fail(result) : NoContent();
    }

    private IActionResult Fail(ResultCommonLogic result) => StatusCode((int)result.HttpStatusCode, result.Message);
}
