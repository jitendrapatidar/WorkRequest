using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WorkRequestTracker.Application.Common.Exceptions;

namespace WorkRequestTracker.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong: {ex}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = HttpStatusCode.InternalServerError;
        var result = string.Empty;

        switch (exception)
        {
            case ValidationException validationException:
                statusCode = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(new { error = "Validation Error", details = validationException.Errors });
                break;
            case NotFoundException notFoundException:
                statusCode = HttpStatusCode.NotFound;
                result = JsonSerializer.Serialize(new { error = "Not Found", message = notFoundException.Message });
                break;
            default:
                statusCode = HttpStatusCode.InternalServerError;
                result = JsonSerializer.Serialize(new { error = "Internal Server Error", message = exception.Message });
                break;
        }

        context.Response.StatusCode = (int)statusCode;
        return context.Response.WriteAsync(result);
    }
}
