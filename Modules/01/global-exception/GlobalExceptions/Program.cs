using GlobalExceptions.Exceptions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = ctx =>
    {
        ctx.ProblemDetails.Extensions["traceId"] = ctx.HttpContext.TraceIdentifier;
        ctx.ProblemDetails.Extensions["timestamp"] = DateTime.UtcNow;
        ctx.ProblemDetails.Instance = $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}";

    };
});

builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();



var app = builder.Build();


app.UseExceptionHandler(new ExceptionHandlerOptions
{
    SuppressDiagnosticsCallback = context =>
        context.Exception is NotFoundException or BadRequestException
});


app.MapGet("/", () => "global exception handling").WithTags("health").WithName("healthcheck").WithSummary("health chekc endpoints");


app.MapGet("/test-error", () =>
{
    throw new Exception("Testing global handler!");
});


app.MapGet("/products/{id:guid}", (Guid id) =>
{
    // Simulate not found scenario
    throw new NotFoundException("Product", id);
})
.WithTags("Products")
.WithName("GetProduct")
.WithSummary("Get a product by ID (always throws NotFoundException for demo)");


app.MapPost("/product", (ProductRequest product) =>
{
    if (string.IsNullOrWhiteSpace(product.Name))
    {
        throw new BadRequestException("productname is required");
    }
} );


app.Run();
public record ProductRequest
(

    string Name
);
