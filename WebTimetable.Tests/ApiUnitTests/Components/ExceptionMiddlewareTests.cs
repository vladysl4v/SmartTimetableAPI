using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebTimetable.Api.Components;
using WebTimetable.Application.Exceptions;

namespace WebTimetable.Tests.ApiUnitTests.Components;

public class ExceptionMiddlewareTests
{
    [Fact]
    public async Task ExceptionMiddleware_ShouldCatchInternalServiceException()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<ExceptionsMiddleware>>();
        var informationForClient = "Whoops exception!";
        var mockRequestDelegate =
            new RequestDelegate(_ => throw new InternalServiceException(informationForClient, "Test exception"));
        
        var middleware = new ExceptionsMiddleware(mockRequestDelegate, mockLogger.Object);
        var context = new DefaultHttpContext { Response = { Body = new MemoryStream() } };

        // Act
        await middleware.InvokeAsync(context);
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
        
        // Assert
        context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        context.Response.ContentType.Should().Be("application/json");
        body.Should().ContainAll("TraceId", "StatusCode", "Message", informationForClient);
    }
    
    [Fact]
    public async Task ExceptionMiddleware_ShouldCatchException()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<ExceptionsMiddleware>>();
        var mockRequestDelegate = new RequestDelegate((_) => throw new Exception("Test exception"));
        var middleware = new ExceptionsMiddleware(mockRequestDelegate, mockLogger.Object);
        var context = new DefaultHttpContext { Response = { Body = new MemoryStream() } };

        // Act
        await middleware.InvokeAsync(context);
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
        
        // Assert
        context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        context.Response.ContentType.Should().Be("application/json");
        body.Should().ContainAll("TraceId", "StatusCode", "Message",
            "An internal error occurred while trying to process the request.");
    }
    
    [Fact]
    public async Task ExceptionMiddleware_ShouldDoNothing()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<ExceptionsMiddleware>>();
        var mockRequestDelegate = new RequestDelegate((_) => Task.CompletedTask);
        var middleware = new ExceptionsMiddleware(mockRequestDelegate, mockLogger.Object);
        var context = new DefaultHttpContext { Response = { Body = new MemoryStream() } };

        // Act
        await middleware.InvokeAsync(context);
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
        
        // Assert
        context.Response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        body.Should().NotContainAny("TraceId", "StatusCode", "Message");
    }
}