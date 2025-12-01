using System.Diagnostics;

namespace ITI.Pharmacy.API.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestPath = context.Request.Path;
            var requestMethod = context.Request.Method;
            var requestQuery = context.Request.QueryString.ToString();

            // Log request
            _logger.LogInformation(
                "Incoming Request: {Method} {Path}{Query} | IP: {IpAddress}",
                requestMethod,
                requestPath,
                requestQuery,
                context.Connection.RemoteIpAddress?.ToString() ?? "Unknown"
            );

            // Capture original response body stream
            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                try
                {
                    await _next(context);
                }
                finally
                {
                    stopwatch.Stop();

                    // Log response
                    responseBody.Seek(0, SeekOrigin.Begin);
                    var responseBodyText = await new StreamReader(responseBody).ReadToEndAsync();
                    responseBody.Seek(0, SeekOrigin.Begin);

                    _logger.LogInformation(
                        "Outgoing Response: {Method} {Path} | Status: {StatusCode} | Duration: {Duration}ms",
                        requestMethod,
                        requestPath,
                        context.Response.StatusCode,
                        stopwatch.ElapsedMilliseconds
                    );

                    // Log response body for debugging (only in development)
                    if (context.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment() 
                        && !string.IsNullOrEmpty(responseBodyText) 
                        && responseBodyText.Length < 1000)
                    {
                        _logger.LogDebug("Response Body: {ResponseBody}", responseBodyText);
                    }

                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
        }
    }
}
