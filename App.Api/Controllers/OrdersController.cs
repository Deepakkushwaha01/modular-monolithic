using App.Core.Command.Orders;
using App.Core.SharedLibrary.Patterns.Mediatr.Abstractions;
using App.Core.SharedLibrary.Patterns.Result;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public OrdersController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderCommand command)
    {
        Result<Guid> result = await _commandDispatcher.ExecuteAsync(command);
        if (!result.IsSuccess)
            return StatusCode((int)result.HttpStatusCode, result.Message);

        return CreatedAtAction(nameof(GetOrder), new { id = result.Value }, new { id = result.Value });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        Result<OrderDto> result = await _queryDispatcher.ExecuteAsync(new GetOrderQuery { Id = id });
        if (!result.IsSuccess)
            return StatusCode((int)result.HttpStatusCode, result.Message);

        return Ok(result.Value);
    }
}
