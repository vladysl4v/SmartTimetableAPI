using Microsoft.AspNetCore.Mvc;
using WebTimetable.Api.Controllers;
using WebTimetable.Application.Services.Abstractions;
using WebTimetable.Contracts.Requests;
using WebTimetable.Contracts.Responses;

namespace WebTimetable.Tests.ApiUnitTests.Controllers;

public class SettingsControllerTests
{
    [Fact]
    public void SettingsController_GetOutageGroups_ReturnsOk()
    {
        // Arrange
        var mockSettingsService = new Mock<ISettingsService>();
        mockSettingsService.Setup(x => x.GetOutageGroups())
            .Returns(new List<KeyValuePair<string, string>>
            {
                new("test", "test")
            });
        var controller = new SettingsController(mockSettingsService.Object);

        // Act
        var result = controller.GetOutageGroups();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).Value.Should().BeOfType<FiltersResponse>();
        var response = (FiltersResponse)((OkObjectResult)result).Value!;
        response.Filters.Should().NotBeNull();
        response.Filters.Should().HaveCount(1);
    }

}