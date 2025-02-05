using Microsoft.AspNetCore.Http;

public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestTimingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        context.Response.OnStarting(() =>
        {
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            context.Response.Headers.Add("X-Response-Time-ms", elapsedMs.ToString());
            return Task.CompletedTask;
        });

        await _next(context);
    }
}
