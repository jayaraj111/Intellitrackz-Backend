using System.Text.Json;

namespace AdminDashboard.Middleware;

public class ExceptionLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;

    public ExceptionLoggingMiddleware(RequestDelegate next, IWebHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var logFolder = Path.Combine(_env.WebRootPath, "ErrorLog");
            Directory.CreateDirectory(logFolder);

            var filePath = Path.Combine(
                logFolder,
                $"errorlog.txt"
            );

            var log = $@"
===========================
Time: {DateTime.UtcNow}
Path: {context.Request.Path}
Method: {context.Request.Method}
Message: {ex.Message}
StackTrace: {ex.StackTrace}
===========================";

            await File.AppendAllTextAsync(filePath, log);

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var response = JsonSerializer.Serialize(new
            {
                success = false,
                message = "Internal Server Error"
            });

            await context.Response.WriteAsync(response);
        }
    }
}
