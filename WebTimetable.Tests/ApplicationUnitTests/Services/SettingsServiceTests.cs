using WebTimetable.Application.Repositories.Abstractions;
using WebTimetable.Application.Services;

namespace WebTimetable.Tests.ApplicationUnitTests.Services;


public class SettingsServiceTests
{
    [Fact]
    public void SettingsService_GetOutageGroups_ReturnsOutages()
    {
        // Arrange
        var mockDbRepository = new Mock<IOutagesRepository>();
        mockDbRepository.Setup(x => x.GetOutageGroups())
            .Returns(new List<KeyValuePair<string, string>>()
            {
                new("Group 1", "Group 1"), 
                new("Group 2", "Group 2"), 
                new("Group 3", "Group 3")
            });
        
        var settingsService = new SettingsService(mockDbRepository.Object);

        // Act
        var outages = settingsService.GetOutageGroups();

        // Assert
        outages.Should().NotBeNull();
        outages.Should().HaveCount(3);
        outages.Should().AllSatisfy(x => x.Value.Should().StartWith("Group"));
    }
}