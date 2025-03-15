using Kurier.CustomGateway.Configs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Kurier.CustomGateway.Middlewares
{
    public class CircuitBreakerMiddleware
    {
        private readonly RequestDelegate next;
        private static readonly object locker = new();

        private static bool isOpen = false;
        private static int failureCount = 0;
        private static DateTime lastFailureTime;

        private readonly CircuitBreakerConfig config;

        public CircuitBreakerMiddleware(RequestDelegate next, IOptions<CircuitBreakerConfig> options)
        {
            this.next = next;
            this.config = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            lock (locker)
            {
                if (isOpen && (DateTime.UtcNow - lastFailureTime).Seconds < config.BreakDuration)
                {
                    context.Response.StatusCode = 503;
                    context.Response.ContentType = "application/json";
                    object value = context.Response.WriteAsync("Service temporarily unavailable due to multiple failures.");
                    return;
                }

                if (isOpen && (DateTime.UtcNow - lastFailureTime).Seconds >= config.BreakDuration)
                {
                    isOpen = false;
                    failureCount = 0;
                }
            }

            try
            {
                await next(context);
            }
            catch (Exception)
            {
                lock (locker)
                {
                    failureCount++;
                    if (failureCount >= config.MaxFailures)
                    {
                        isOpen = true;
                        lastFailureTime = DateTime.UtcNow;
                    }
                }

                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Internal Server Error");
            }
        }
    }
}
