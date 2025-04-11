using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;

namespace Kurier.Common.ApiConfiguration
{
    public static class ApiConfigurationHelper
    {
        public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
        {
            builder.Logging.ClearProviders();

            builder.Host.UseSerilog((context, config) =>
                config.ReadFrom.Configuration(context.Configuration));

            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields =
                    HttpLoggingFields.Request |
                    HttpLoggingFields.Response;
                logging.RequestBodyLogLimit = 32 * 1024; // 32 KB
                logging.ResponseBodyLogLimit = 32 * 1024; // 32 KB
            });

            return builder;
        }
    }
}
