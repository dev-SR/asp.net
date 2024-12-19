using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Logger.Contract;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

// Throwing not found error: 
public abstract class NotFoundException : Exception
{
    protected NotFoundException(string message) : base(message) { }
}

public class ThrowNotFound : NotFoundException
{
    public ThrowNotFound() : base("Not found")
    {
    }
}
public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public Dictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>>();

    public override string ToString() => JsonSerializer.Serialize(this);
}


[Route("api/errors")]
[ApiController]
public class ErrorController : ControllerBase
{
    private readonly ILoggerManager _logger;
    public ErrorController(ILoggerManager logger)
    {
        _logger = logger;

    }

    [HttpGet("notFound")]
    public ActionResult NotFoundMethod()
    {
        throw new ThrowNotFound();
    }
    [HttpGet("serverError")]
    public ActionResult ServerErrorMethod()
    {
        throw new Exception();
    }

    [HttpGet("badRequest")]
    public ActionResult BadRequestMethod()
    {
        return BadRequest(new ErrorDetails() { StatusCode = 400, Message = "Bad Request" });
    }
    [HttpGet("badRequest/{id}")]
    public ActionResult BadRequestByIdMethod([FromRoute] Guid id)
    {
        _logger.LogError($"Invalid Parameter{id}");
        return Ok();
    }

    [HttpPost("creationValidation")]
    public IActionResult POST([FromBody] Company data)
    {
        // re-run validation
        data.Address = "";
        ModelState.ClearValidationState(nameof(Company));
        if (!TryValidateModel(data, nameof(Company)))
            return BadRequest(new ErrorDetails()
            {
                StatusCode = 400,
                Message = "Bad Request",
                Errors = ModelState.ToDictionary(
                                kvp => kvp.Key,
                                kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToList() ?? new List<string>()
                            )
            });

        return Ok(data);
    }

}

public class Company
{

    [Required(ErrorMessage = "Company name is a required field.")]
    [MinLength(6, ErrorMessage = "Minimum length for the Name is 6 characters.")]
    public string? Name { get; set; }
    [Required(ErrorMessage = "Company address is a required field.")]
    [MinLength(6, ErrorMessage = "Minimum length for the Address is 6 characters")]
    public string? Address { get; set; }
}
