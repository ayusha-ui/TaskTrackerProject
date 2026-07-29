using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class PerformanceLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<PerformanceLoggingMiddleware> _logger;

    // The next middleware component is injected via the constructor
    public PerformanceLoggingMiddleware(RequestDelegate next, ILogger<PerformanceLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        // 1. Logic BEFORE the next middleware (Request path)
        _logger.LogInformation("Incoming request: {Method} {Path}", context.Request.Method, context.Request.Path);

        // 2. Call the next middleware in the pipeline
        await _next(context);

        // 3. Logic AFTER the next middleware (Response path)
        stopwatch.Stop();
        _logger.LogInformation("Outgoing response: {StatusCode} processed in {ElapsedMs}ms",
            context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
    }
}