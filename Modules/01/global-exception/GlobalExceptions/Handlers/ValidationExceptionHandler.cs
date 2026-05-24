using Microsoft.AspNetCore.Mvc;

namespace GlobalExceptions.Exceptions;

public sealed class ValidationExceptionHandler(ILogger<ValidationExceptionHandler> logger)
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        if(exception is not ValidationException validation)
        {
            return false;
        }

        logger.LogWarning("Validation Failed {Message}", validation.Message);

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            await httpContext.Response.WriteAsJsonAsync(new ValidationProblemDetails(validation.Errors)
        {
            Status = 400,
            Title = "Validation Failed",
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Instance = httpContext.Request.Path
        }, cancellationToken);

        return true;
    }
}