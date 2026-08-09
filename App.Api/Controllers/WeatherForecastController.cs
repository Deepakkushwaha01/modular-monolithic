using Microsoft.AspNetCore.Mvc;

namespace Dotnet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        FakeResponse[] data = new[]
        {
            new FakeResponse
            {
                Id = 1,
                Name = "John Doe",
                Email = "john@example.com"
            },
            new FakeResponse
            {
                Id = 2,
                Name = "Jane Smith",
                Email = "jane@example.com"
            }
        };

        return Ok(data);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        FakeResponse data = new FakeResponse
        {
            Id = id,
            Name = $"User {id}",
            Email = $"user{id}@example.com"
        };

        return Ok(data);
    }

    [HttpPost]
    public IActionResult Create(FakeRequest request)
    {
        return Ok(new
        {
            Message = "Record created successfully.",
            Data = request
        });
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, FakeRequest request)
    {
        return Ok(new
        {
            Message = $"Record {id} updated successfully.",
            Data = request
        });
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        return Ok(new
        {
            Message = $"Record {id} deleted successfully."
        });
    }
}

public class FakeRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class FakeResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}