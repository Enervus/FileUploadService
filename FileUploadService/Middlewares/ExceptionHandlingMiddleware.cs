﻿using FileUploadService.Domain.Result;
using System.Net;
using ILogger = Serilog.ILogger;

namespace FileUploadService.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                await HandlingExceptionAsync(httpContext,exception);
            }
        }

        private async Task HandlingExceptionAsync(HttpContext httpContext, Exception exception)
        {
            _logger.Error(exception, exception.Message);

            var errorMessage = exception.Message;
            var response = exception switch
            {
                UnauthorizedAccessException _ => new BaseResult { ErrorMessage = errorMessage, ErrorCode = (int)HttpStatusCode.Unauthorized },
                _ => new BaseResult { ErrorMessage = "Internal server error. Please retry later", ErrorCode = (int)HttpStatusCode.InternalServerError },
            };
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)response.ErrorCode;
            await httpContext.Response.WriteAsJsonAsync(response);
        }
    }
}
