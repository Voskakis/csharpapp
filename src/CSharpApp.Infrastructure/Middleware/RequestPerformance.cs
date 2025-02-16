using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace CSharpApp.Infrastructure.Middleware
{
    public class RequestPerformace
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestPerformace> _logger;

        public RequestPerformace(RequestDelegate next, ILogger<RequestPerformace> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation("{Method} {Path} responded in {ElapsedMilliseconds}ms", context.Request.Method, context.Request.Path, stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
