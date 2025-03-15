using Kurier.CustomGateway.Configs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Kurier.CustomGateway.Middlewares
{
    public class ProxyMiddleware
    {
        private readonly RequestDelegate next;
        private readonly HttpClient httpClient;
        private readonly ApiGatewayConfig config;

        public ProxyMiddleware(RequestDelegate next, HttpClient httpClient, IOptions<ApiGatewayConfig> options)
        {
            this.next = next;
            this.httpClient = httpClient;
            this.config = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value;

            var route = config.Routes.FirstOrDefault(r => path.StartsWith(r.Path));
            if (route == null)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("Route not found");
                return;
            }

            var downstreamUrl = route.DownstreamUrl + path.Substring(route.Path.Length);
            var requestMessage = new HttpRequestMessage(new HttpMethod(context.Request.Method), downstreamUrl);

            var response = await httpClient.SendAsync(requestMessage);
            context.Response.StatusCode = (int)response.StatusCode;
            await context.Response.WriteAsync(await response.Content.ReadAsStringAsync());
        }
    }
}
