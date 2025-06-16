using System.Text.Json;

namespace BlogApi.Middlewares
{
    public class NotFoundMiddleware
    {
        private readonly RequestDelegate _next;

        public NotFoundMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";

                var response = new
                {
                    error = "Endpoint not found",
                    statusCode = 404,
                    timestamp = DateTime.UtcNow
                };

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
