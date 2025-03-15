using Kurier.CustomGateway.Configs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Kurier.CustomGateway.Middlewares
{
    public class SwaggerAggregatorMiddleware
    {
        private readonly RequestDelegate next;
        private readonly HttpClient httpClient;
        private readonly ApiGatewayConfig config;

        public SwaggerAggregatorMiddleware(RequestDelegate next, HttpClient httpClient, IOptions<ApiGatewayConfig> options)
        {
            this.next = next;
            this.httpClient = httpClient;
            this.config = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Value.Equals("/swagger/v1/swagger.json", StringComparison.OrdinalIgnoreCase))
            {
                var aggregatedDoc = await AggregateSwaggerDocsAsync();
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(aggregatedDoc.ToString());
                return;
            }

            await next(context);
        }

        private async Task<JObject> AggregateSwaggerDocsAsync()
        {
            var aggregatedPaths = new JObject();
            var aggregatedSchemas = new JObject();

            foreach (var route in config.Routes)
            {
                if (string.IsNullOrEmpty(route.SwaggerUrl)) continue;

                try
                {
                    var serviceSwagger = await httpClient.GetStringAsync(route.SwaggerUrl);
                    var serviceDoc = JObject.Parse(serviceSwagger);

                    // Объединяем пути
                    if (serviceDoc["paths"] is JObject paths)
                    {
                        foreach (var path in paths.Properties())
                        {
                            aggregatedPaths[path.Name] = path.Value;
                        }
                    }

                    // Объединяем схемы (models)
                    if (serviceDoc["components"]?["schemas"] is JObject schemas)
                    {
                        foreach (var schema in schemas.Properties())
                        {
                            aggregatedSchemas[schema.Name] = schema.Value;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка загрузки Swagger из {route.SwaggerUrl}: {ex.Message}");
                }
            }

            return new JObject
            {
                ["openapi"] = "3.0.1",
                ["info"] = new JObject
                {
                    ["title"] = "API Gateway",
                    ["version"] = "v1"
                },
                ["paths"] = aggregatedPaths,
                ["components"] = new JObject { ["schemas"] = aggregatedSchemas }
            };
        }
    }
}
