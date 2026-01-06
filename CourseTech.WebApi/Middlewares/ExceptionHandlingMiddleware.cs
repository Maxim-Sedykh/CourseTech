using CourseTech.Domain.Result;
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
#if DEBUG
            Debugger.Break();
#endif

            await HandleExceptionAsync(httpContext, ex);
        }
    }

    /// <summary>
    /// Обработать ошибку и отдать соответствующий response
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        logger.Error(exception, exception.Message);

        var errorMessage = exception.Message;
        var response = exception switch
        {
            UnauthorizedAccessException => BaseResult.Failure((int)HttpStatusCode.Unauthorized, errorMessage),
            _ => BaseResult.Failure((int)HttpStatusCode.InternalServerError, errorMessage),
        };

        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        httpContext.Response.StatusCode = (int)response.Error.Code;
        await httpContext.Response.WriteAsJsonAsync(response);
    }
}
