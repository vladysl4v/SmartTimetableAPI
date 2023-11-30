using Microsoft.AspNetCore.Mvc;
using WebTimetable.Api.Controllers;
using WebTimetable.Application.Services.Abstractions;
using WebTimetable.Contracts.Requests;
using WebTimetable.Contracts.Responses;

namespace WebTimetable.Tests.ControllersTests;

public class SettingsControllerTests
{
    [Fact]
    public async Task SettingsController_GetFilters_ReturnsOk()
    {
        // Arrange
        var mockSettingsService = new Mock<ISettingsService>();
        mockSettingsService.Setup(x => x.GetFilters(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dictionary<string, List<KeyValuePair<string, string>>>()
            {
                { "courses", new List<KeyValuePair<string, string>>() },
                { "educForms", new List<KeyValuePair<string, string>>() },
                { "faculties", new List<KeyValuePair<string, string>>() },
            });
        var controller = new SettingsController(mockSettingsService.Object);

        // Act
        var result = await controller.GetFilters(CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).Value.Should().BeOfType<FiltersResponse>();
        var response = (FiltersResponse)((OkObjectResult)result).Value!;
        response.Filters.Should().NotBeNull();
        response.Filters.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task SettingsController_GetStudyGroups_ReturnsOk()
    {
        // Arrange
        var mockSettingsService = new Mock<ISettingsService>();
        mockSettingsService.Setup(x => x.GetStudyGroups(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<KeyValuePair<string, string>>()
            {
                new("test1", "1"),
                new("test2", "2"),
                new("test3", "3"),
            });
        var controller = new SettingsController(mockSettingsService.Object);
        var request = new StudyGroupsRequest
        {
            Course = 1,
            EducationForm = 1,
            Faculty = "test"
        };

        // Act
        var result = await controller.GetStudyGroups(request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).Value.Should().BeOfType<StudyGroupsResponse>();
        var response = (StudyGroupsResponse)((OkObjectResult)result).Value!;
        response.StudyGroups.Should().NotBeNull();
        response.StudyGroups.Should().HaveCount(3);
    }
}