using MiddlewareSamples.Api.Middlewares;

namespace MiddlewareSamples.Api.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestLoggingMiddleware>();
        }

        public static IApplicationBuilder useMaintenance(this IApplicationBuilder app )
        {
            return app.UseMiddleware<MaintenanceMiddleware>();
        }

        public static IApplicationBuilder useCorrealtionId(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CorrelationIdMiddleware>();
        }
    }
}