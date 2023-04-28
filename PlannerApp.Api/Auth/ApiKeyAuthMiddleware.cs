namespace PlannerApp.Api.Auth
{
    public class ApiKeyAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public ApiKeyAuthMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if(!context.Request.Path.StartsWithSegments("/swagger"))
            {
                if (!context.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("API Key missing!");
                    return;
                }

                var apiKey = _configuration[AuthConstants.ApiKeySectionName];
                if (!apiKey.Equals(extractedApiKey))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid API Key!");
                    return;
                }
            }          

            await _next(context);
        }
    }
}
