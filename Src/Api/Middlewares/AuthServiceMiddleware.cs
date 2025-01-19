namespace Kurier.Api.Middlewares
{
    public class AuthServiceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthServiceMiddleware(RequestDelegate next, IHttpClientFactory httpClientFactory)
        {
            _next = next;
            _httpClientFactory = httpClientFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Authorization header is missing.");
                return;
            }

            var token = authHeader.ToString().Split(" ").Last();
            var client = _httpClientFactory.CreateClient();

            try
            {
                // STUB
                // Запрос к сервису клиента/курьера ИЛИ к единому сервису пользователей на валидацию токена

                //var response = await client.PostAsJsonAsync("http://auth-service:5000/api/auth/validate", token);

                //if (!response.IsSuccessStatusCode)
                //{
                //    context.Response.StatusCode = 401; // Unauthorized
                //    await context.Response.WriteAsync("Invalid or expired token.");
                //    return;
                //}

                //var validateResponse = await response.Content.ReadFromJsonAsync<ValidateTokenResponse>();
                //context.Items["UserId"] = validateResponse.ClientId;
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500; // Internal Server Error
                await context.Response.WriteAsync("Error validating token: " + ex.Message);
                return;
            }

            await _next(context);
        }
    }
}
