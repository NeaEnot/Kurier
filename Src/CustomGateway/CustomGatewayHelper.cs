using Kurier.CustomGateway.Configs;
using Kurier.CustomGateway.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kurier.CustomGateway
{
    public static class CustomGatewayHelper
    {
        public static IServiceCollection AddCustomGatewayServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiGatewayConfig>(configuration.GetSection("Routes"));
            services.Configure<CircuitBreakerConfig>(configuration.GetSection("CircuitBreaker"));

            services.AddSingleton<ProxyMiddleware>();
            services.AddSingleton<SwaggerAggregatorMiddleware>();

            return services;
        }

        public static IApplicationBuilder UseCustomGateway(this IApplicationBuilder app)
        {
            app.UseMiddleware<ProxyMiddleware>();
            //app.UseMiddleware<SwaggerAggregatorMiddleware>();
            app.UseMiddleware<CircuitBreakerMiddleware>();

            return app;
        }
    }
}
