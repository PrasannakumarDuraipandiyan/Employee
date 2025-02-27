namespace EmployeeWebAPI.Middleware;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private const string API_KEY_HEADER = "X-Api-Key";

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task Invoke(HttpContext context)
    {
        // Skip validation for Swagger UI or any route that contains "/swagger"
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            await _next(context); // Skip API key validation for Swagger routes
            return;
        }

        // Get the API key from configuration
        var validApiKey = _configuration["ApiSettings:ApiKey"];

        if (!context.Request.Headers.TryGetValue(API_KEY_HEADER, out var extractedApiKey) ||
            extractedApiKey != validApiKey)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Invalid API Key");
            return;
        }

        await _next(context);
    }
}

