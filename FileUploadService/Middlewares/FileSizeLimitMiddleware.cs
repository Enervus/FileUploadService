using ILogger = Serilog.ILogger;
using System.Net;
using System;

namespace FileUploadService.Middlewares
{
    public class FileSizeLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly long _maxFileSize;
        private readonly ILogger _logger;
        public FileSizeLimitMiddleware(RequestDelegate next, long maxFileSize, ILogger logger)
        {
            _next = next;
            _maxFileSize = maxFileSize;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.Request.ContentType?.ToLower().Contains("multipart/form-data") == true)
            {
                var contentLength = httpContext.Request.ContentLength;
                if (contentLength.HasValue && contentLength > _maxFileSize)
                {
                    _logger.Error("File size exceeds the limit (100MB)");
                    httpContext.Response.ContentType = "application/json";
                    httpContext.Response.StatusCode = (int)StatusCodes.Status413PayloadTooLarge;
                    await httpContext.Response.WriteAsJsonAsync("File size exceeds the limit (100MB)");
                    return;
                }
            }
            await _next(httpContext);
        }
    }
}
