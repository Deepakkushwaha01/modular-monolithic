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
[Route("api/v{version:apiVersion}/user-claims")]
public sealed class UserClaimsController(ICommandDispatcher commands, IQueryDispatcher queries) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        Result<List<UserClaim>> result = await queries.ExecuteAsync(new GetUserClaimsQuery());
        return result.IsFailure ? Fail(result) : Ok(result.Value.Select(UserClaimResponse.From));
    }

    [HttpGet("{uid:guid}")]
    public async Task<IActionResult> Get(Guid uid)
    {
        Result<UserClaim> result = await queries.ExecuteAsync(new GetUserClaimByUidQuery(uid));
        return result.IsFailure ? Fail(result) : Ok(UserClaimResponse.From(result.Value));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserClaimRequest request)
    {
        Result<Guid> result = await commands.ExecuteAsync(new CreateUserClaimCommand(request.UserId!.Value, request.ClaimTypeId!.Value, request.ClaimValue));
        return result.IsFailure ? Fail(result) : CreatedAtAction(nameof(Get), new { uid = result.Value }, new { uid = result.Value });
    }

    [HttpPut("{uid:guid}")]
    public async Task<IActionResult> Update(Guid uid, [FromBody] UserClaimRequest request)
    {
        Result<Guid> result = await commands.ExecuteAsync(new UpdateUserClaimCommand(uid, request.ClaimValue));
        return result.IsFailure ? Fail(result) : NoContent();
    }

    [HttpDelete("{uid:guid}")]
    public async Task<IActionResult> Delete(Guid uid)
    {
        Result result = await commands.ExecuteAsync(new DeleteUserClaimCommand(uid));
        return result.IsFailure ? Fail(result) : NoContent();
    }

    private IActionResult Fail(ResultCommonLogic result) => StatusCode((int)result.HttpStatusCode, result.Message);
}
