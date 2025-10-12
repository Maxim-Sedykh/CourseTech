using CourseTech.Domain;
using System.Diagnostics;
using System.Net;
using System.Net.Mime;
using ILogger = Serilog.ILogger;

namespace CourseTech.WebApi.Middlewares;

/// <summary>
/// Единый обработчик ошибок (глобальный try - catch)
/// </summary>
public class ExceptionHandlingMiddleware(ILogger logger, RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            Debugger.Break();

            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        logger.Error(exception, exception.Message);

        var errorMessage = exception.Message;
        var response = exception switch
        {
            UnauthorizedAccessException => Result.Failure($"{(int)HttpStatusCode.Unauthorized} - {errorMessage}"),
            _ => Result.Failure($"{(int)HttpStatusCode.InternalServerError} - {errorMessage}"),
        };

        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        httpContext.Response.StatusCode = 500;
        await httpContext.Response.WriteAsJsonAsync(response);
    }
}
