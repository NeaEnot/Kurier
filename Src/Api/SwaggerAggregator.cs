using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Kurier.Api
{
    public class SwaggerAggregator
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration configuration;

        public SwaggerAggregator(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
        }

        public async Task<JObject> AggregateSwaggerDocsAsync()
        {
            var routes = configuration.GetSection("Routes");
            var client = httpClientFactory.CreateClient();

            var aggregatedPaths = new JObject();
            var aggregatedComponents = new JObject();

            foreach (var route in routes.GetChildren())
            {
                string protocol = route.GetSection("DownstreamScheme").Value;
                string host = route.GetSection("DownstreamHostAndPorts").GetSection("Host").Value;
                int port = int.Parse(route.GetSection("DownstreamHostAndPorts").GetSection("Port").Value);

                var serviceSwagger = await client.GetStringAsync($"{protocol}://{host}:{port}/swagger/v1/swagger.json");
                var serviceDoc = JObject.Parse(serviceSwagger);

                if (serviceDoc["paths"] is JObject paths)
                    foreach (var path in paths.Properties())
                        aggregatedPaths[path.Name] = path.Value;

                // Объединяем components (schemas в OpenAPI 3.0)
                if (serviceDoc["components"]?["schemas"] is JObject schemas)
                    foreach (var schema in schemas.Properties())
                        aggregatedComponents[schema.Name] = schema.Value;
            }

            // Объединяем данные (упрощённый пример)
            var aggregatedDoc = new JObject
            {
                ["openapi"] = "3.0.1",
                ["info"] = new JObject
                {
                    ["title"] = "API Gateway",
                    ["version"] = "v1"
                },
                ["paths"] = new JObject(aggregatedPaths["paths"]),
                ["components"] = new JObject
                {
                    ["schemas"] = aggregatedComponents
                }
            };

            return aggregatedDoc;
        }
    }
}
