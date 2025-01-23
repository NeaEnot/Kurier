using Kurier.Common.Models;

namespace Kurier.Api.Middlewares
{
    public class AuthServiceMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration configuration;

        public AuthServiceMiddleware(RequestDelegate next, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this.next = next;
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Authorization header is missing.");
                return;
            }

            string tokenId = authHeader.ToString().Split(" ").Last();
            HttpClient client = httpClientFactory.CreateClient();

            try
            {
                string url = configuration["UserValidateUrl"];
                HttpResponseMessage response = await client.PostAsJsonAsync(url, tokenId);

                if (!response.IsSuccessStatusCode)
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    await context.Response.WriteAsync("Invalid or expired token.");
                    return;
                }

                UserAuthToken token = await response.Content.ReadFromJsonAsync<UserAuthToken>();
                context.Items["UserToken"] = token;
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500; // Internal Server Error
                await context.Response.WriteAsync("Error validating token: " + ex.Message);
                return;
            }

            await next(context);
        }
    }
}
