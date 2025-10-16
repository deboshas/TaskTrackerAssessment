using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace TaskTracker.API.Controllers;

[ApiController]
[ExcludeFromCodeCoverage]
public class ApiController : ControllerBase
{
    /// <summary>  
    /// Handles a list of errors and returns an appropriate IActionResult.  
    /// </summary>  
    /// <param name="errors">A list of errors to process.</param>  
    /// <returns>An IActionResult representing the error response.</returns>  
    protected IActionResult Problem(List<Error> errors)
    {
        if (errors?.Count == 0)
        {
            return Problem();
        }

        if (errors!=null && errors.Any() &&   errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidateProblems(errors);
        }

        return Problem(errors[0]);
    }

    /// <summary>  
    /// Handles a single error and returns an appropriate IActionResult.  
    /// </summary>  
    /// <param name="error">The error to process.</param>  
    /// <returns>An IActionResult representing the error response.</returns>  
    protected IActionResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(statusCode: statusCode, title: error.Description);
    }

    /// <summary>  
    /// Converts a list of validation errors into a ValidationProblem response.  
    /// </summary>  
    /// <param name="erros">A list of validation errors.</param>  
    /// <returns>An IActionResult representing the validation problem response.</returns>  
    private IActionResult ValidateProblems(List<Error> erros)
    {
        var modelStateDictionary = new ModelStateDictionary();
        foreach (var error in erros)
        {
            modelStateDictionary.AddModelError(error.Code, error.Description);
        }
        return ValidationProblem(modelStateDictionary);
    }

    /// <summary>  
    /// Returns a Created (201) response with the provided value.  
    /// </summary>  
    /// <typeparam name="T">The type of the value to return.</typeparam>  
    /// <param name="value">The value to include in the response.</param>  
    /// <returns>An ObjectResult with a 201 status code.</returns>  
    protected ObjectResult Created<T>(T value)
    {
        return StatusCode((int)HttpStatusCode.Created, value);
    }
}
