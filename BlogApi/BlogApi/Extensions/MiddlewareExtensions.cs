using BlogApi.Middlewares;

namespace BlogApi.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomNotFound(this IApplicationBuilder app)
        {
            return app.UseMiddleware<NotFoundMiddleware>();
        }

        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
