using MiddlewareSamples.Api.Extensions;
using MiddlewareSamples.Api.Middlewares;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddTransient<CorrelationIdMiddleware>();

var app = builder.Build();


// app.MapGet("/", (HttpContext ctx) =>
// {
//     var method = ctx.Request.Method;
//     var userAgent = ctx.Request.Headers["User-Agent"];

//     return $"Method: {method} {userAgent}";
// });

// app.MapGet("/a", (ILogger<Program> logger) =>
// {
//     logger.LogInformation("Home endpoint was called");

//     return "Hello";
// });

// app.UseMiddleware<RequestLoggingMiddleware>();


app.useMaintenance();
app.useCorrealtionId();
app.UseRequestLogging();




 app.MapGet("/", () => "Hello World!");

 app.Run();
