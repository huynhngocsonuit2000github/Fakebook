using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fakebook.AuthService.Middlewares;
public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        // Log the exception
        Console.WriteLine($"Unhandled Exception: {context.Exception.Message}");

        // Create a structured error response
        var errorResponse = new
        {
            StatusCode = 500,
            Message = "An internal server error occurred.",
            Error = context.Exception.Message
        };

        // Set the response
        context.Result = new ObjectResult(errorResponse)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true; // Mark exception as handled
    }
}
