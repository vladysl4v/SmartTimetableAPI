using Microsoft.Graph;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Moq;

namespace WebTimetable.Tests.TestingUtilities;

public class MockGraphClientFactory
{
    private readonly Mock<IRequestAdapter> _requestAdapter = new();
    
    public MockGraphClientFactory Setup<T>(T requiredResponse) where T : IParsable
    {
        _requestAdapter.Setup(a => a.SendAsync(It.IsAny<RequestInformation>(), It.IsAny<ParsableFactory<T>>(),
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => requiredResponse);
        return this;
    }
    
    public GraphServiceClient CreateClient()
    {
        return new GraphServiceClient(_requestAdapter.Object, "");
    }
}