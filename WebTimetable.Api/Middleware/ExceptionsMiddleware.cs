using System.Net;
using WebTimetable.Application.Exceptions;
using WebTimetable.Contracts.Responses;


namespace WebTimetable.Api.Middleware;

public class ExceptionsMiddleware
{
    private readonly ILogger<ExceptionsMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionsMiddleware(RequestDelegate next, ILogger<ExceptionsMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (InternalServiceException ex)
        {
            var traceId = Guid.NewGuid();
            _logger.LogCritical($"[TraceID {traceId}]: {ex}");
            await HandleExceptionAsync(context, traceId, ex.InformationForClient);
        }
        catch (Exception ex)
        {
            var traceId = Guid.NewGuid();
            _logger.LogError($"[TraceID {traceId}]: {ex}");
            await HandleExceptionAsync(context, traceId, "An internal error occurred while trying to process the request.");
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Guid traceId, string messageForClient)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsync(new ErrorDetailsResponse
        {
            TraceId = traceId,
            StatusCode = context.Response.StatusCode,
            Message = messageForClient
        }.ToString());
    }
}