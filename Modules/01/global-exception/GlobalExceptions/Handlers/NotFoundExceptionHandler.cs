using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GlobalExceptions.Exceptions;


public sealed class NotFoundExceptionHandler(ILogger<NotFoundExceptionHandler> logger): IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        if(exception is not NotFoundException notFound)
        {
            return false;
        }
        logger.LogWarning("Resource not found", notFound.Message);


        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

        await httpContext.Response.WriteAsJsonAsync( new ProblemDetails
        {
            Status = 404,
            Title = "Resource not found",
            Detail = notFound.Message,
            Type =  "https://tools.ietf.org/html/rfc9110#section-15.5.5",
            Instance = httpContext.Request.Path
        }, cancellationToken);

        return true;
    }
}

