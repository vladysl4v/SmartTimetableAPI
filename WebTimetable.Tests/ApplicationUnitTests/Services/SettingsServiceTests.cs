using System.Linq.Expressions;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Repositories;
using WebTimetable.Application.Services;

namespace WebTimetable.Tests.ApplicationUnitTests.Services;


public class SettingsServiceTests
{
    [Fact]
    public void SettingsService_GetOutageGroups_ReturnsOutages()
    {
        // Arrange
        var mockDbRepository = new Mock<IRepository<OutageEntity>>();
        mockDbRepository.Setup(x => x.Where(It.IsAny<Expression<Func<OutageEntity, bool>>>()))
            .Returns(new List<OutageEntity>()
            {
                new() { Group = "Group 1" }, 
                new() { Group = "Group 1" }, 
                new() { Group = "Group 2" }, 
                new() { Group = "Group 2" }, 
                new() { Group = "Group 3" }, 
                new() { Group = "Group 3" }
            }.AsQueryable());
        
        var settingsService = new SettingsService(mockDbRepository.Object);

        // Act
        var outages = settingsService.GetOutageGroups();

        // Assert
        outages.Should().NotBeNull();
        outages.Should().HaveCount(3);
        outages.Should().AllSatisfy(x => x.Value.Should().StartWith("Group"));
    }
}