namespace VendorHub.Controllers.Common;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using VendorHub.Application.Common.Models;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    private ISender? _sender;
    protected ISender Sender => _sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected IActionResult HandleResult(Result result)
    {
        if (result.IsSuccess)
            return NoContent();

        return HandleFailure(result.Error);
    }

    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
            return Ok(result.Value);

        return HandleFailure(result.Error);
    }

    private IActionResult HandleFailure(Error error)
    {
        return error.code switch
        {
            var code when code.Contains("NotFound") => NotFound(new ProblemDetails
            {
                Title = "Resource Not Found",
                Detail = error.Description,
                Status = StatusCodes.Status404NotFound
            }),
            var code when code.Contains("Conflict") || code.Contains("AlreadyInUse") => Conflict(new ProblemDetails
            {
                Title = "Conflict",
                Detail = error.Description,
                Status = StatusCodes.Status409Conflict
            }),
            _ => BadRequest(new ProblemDetails
            {
                Title = "Validation or Business Rule Error",
                Detail = error.Description,
                Status = StatusCodes.Status400BadRequest
            })
        };
    }
}
