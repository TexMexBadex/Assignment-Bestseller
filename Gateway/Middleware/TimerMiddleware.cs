using System.Diagnostics;

namespace Gateway.Middleware;

public class TimerMiddleware(RequestDelegate next, ILogger<TimerMiddleware> logger)
{
  private readonly RequestDelegate _next = next;
  private readonly ILogger<TimerMiddleware> _logger = logger;

  public async Task InvokeAsync(HttpContext context)
  {
    var stopwatch = Stopwatch.StartNew();

    await _next(context);
    stopwatch.Stop();

    _logger.LogInformation("[{Method}] [{Path}] responded [{StatusCode}] in [{ElapsedMilliseconds}ms]",
    context.Request.Method,
    context.Request.Path,
    context.Response.StatusCode,
    stopwatch.ElapsedMilliseconds);

  }
}
