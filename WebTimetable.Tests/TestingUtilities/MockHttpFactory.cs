using System.Linq.Expressions;
using System.Net;
using Moq.Protected;

namespace WebTimetable.Tests.TestingUtilities;

public class MockHttpFactory : IHttpClientFactory
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler = new();
    
    public HttpClient CreateClient(string name)
    {
        return new HttpClient(_mockHttpMessageHandler.Object);
    }
    
    public MockHttpFactory Setup(string mockResponseData)
    {
        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK);
        mockResponse.Content = new StringContent(mockResponseData);
        
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()).ReturnsAsync(mockResponse);
        return this;
    }
    
    public MockHttpFactory Setup(Expression<Func<HttpRequestMessage, bool>> requestFilter, string mockResponseData)
    {
        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK);
        mockResponse.Content = new StringContent(mockResponseData);

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is(requestFilter),
                ItExpr.IsAny<CancellationToken>()).ReturnsAsync(mockResponse);
        return this;
    }
}