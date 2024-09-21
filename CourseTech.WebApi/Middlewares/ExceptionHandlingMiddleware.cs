using CourseTech.Domain.Result;
using System.Diagnostics;
using System.Net;
using ILogger = Serilog.ILogger;

namespace CourseTech.WebApi.Middlewares;

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
        catch (Exception ex)
        {
            Debugger.Break();

            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        _logger.Error(exception, exception.Message);

        var errorMessage = exception.Message;
        var response = exception switch
        {
            UnauthorizedAccessException => BaseResult.Failure((int)HttpStatusCode.Unauthorized, errorMessage),
            _ => BaseResult.Failure((int)HttpStatusCode.InternalServerError, errorMessage),
        };

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)response.Error.Code;
        await httpContext.Response.WriteAsJsonAsync(response);
    }
}
