using App.API.Controllers.Identity.Contracts;
using App.API.Settings;
using App.Core.Command.Identity.Claims;
using App.Core.Domain.Identity.Claims;
using App.Core.Queries.Identity.Claims;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers.Identity;

[ApiController]
[ApiVersion(1.0)]
[SwaggerModule(SwaggerDocuments.Identity)]
[Route("api/v{version:apiVersion}/claim-types")]
public sealed class ClaimTypesController(ICommandDispatcher commands, IQueryDispatcher queries) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll(
        [FromQuery] bool includeRoleClaims = false,
        [FromQuery] bool includeUserClaims = false)
    {
        Result<List<ClaimType>> result = await queries.ExecuteAsync(
            new GetClaimTypesQuery(includeRoleClaims, includeUserClaims));

        return result.IsFailure
            ? Fail(result)
            : Ok(result.Value.Select(x => ClaimTypeResponse.From(x, includeRoleClaims, includeUserClaims)));
    }

    [HttpGet("{uid:guid}")]
    public async Task<IActionResult> Get(
        Guid uid,
        [FromQuery] bool includeRoleClaims = false,
        [FromQuery] bool includeUserClaims = false)
    {
        Result<ClaimType> result = await queries.ExecuteAsync(
            new GetClaimTypeByUidQuery(uid, includeRoleClaims, includeUserClaims));

        return result.IsFailure
            ? Fail(result)
            : Ok(ClaimTypeResponse.From(result.Value, includeRoleClaims, includeUserClaims));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ClaimTypeRequest request)
    {
        Result<Guid> result = await commands.ExecuteAsync(
            new CreateClaimTypeCommand(request.Name, request.Description, request.DataType!.Value, request.Module));

        return result.IsFailure
            ? Fail(result)
            : CreatedAtAction(nameof(Get), new { uid = result.Value }, new { uid = result.Value });
    }

    [HttpPut("{uid:guid}")]
    public async Task<IActionResult> Update(Guid uid, [FromBody] ClaimTypeRequest request)
    {
        Result<Guid> result = await commands.ExecuteAsync(
            new UpdateClaimTypeCommand(uid, request.Name, request.Description, request.DataType!.Value, request.Module, request.IsActive));

        return result.IsFailure
            ? Fail(result)
            : NoContent();
    }

    [HttpDelete("{uid:guid}")]
    public async Task<IActionResult> Delete(Guid uid)
    {
        Result result = await commands.ExecuteAsync(new DeleteClaimTypeCommand(uid));

        return result.IsFailure
            ? Fail(result)
            : NoContent();
    }

    private IActionResult Fail(ResultCommonLogic result) =>
        StatusCode((int)result.HttpStatusCode, result.Message);
}
