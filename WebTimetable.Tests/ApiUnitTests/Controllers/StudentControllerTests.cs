using Microsoft.AspNetCore.Mvc;
using WebTimetable.Api.Controllers;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Models;
using WebTimetable.Application.Services.Abstractions;
using WebTimetable.Contracts.Requests;
using WebTimetable.Contracts.Responses;

namespace WebTimetable.Tests.ApiUnitTests.Controllers;

public class StudentControllerTests
{
    [Fact]
    public async Task StudentController_GetAnonymousSchedule_ReturnsOk()
    {
        // Arrange
        var mockScheduleService = new Mock<IStudentService>();
        var mockUsersService = new Mock<IUsersService>();
        mockScheduleService.Setup(x => x.GetScheduleAsync(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<UserEntity>()))
            .ReturnsAsync(new List<StudentLesson>
            {
                new() { Id = Guid.NewGuid(), Discipline = "Test subject" }
            });
        var request = new StudentScheduleRequest
        {
            Date = DateTime.Now.ToString("yyyy-MM-dd"),
            StudyGroup = "test",
            OutageGroup = "test"
        };
        var controller = new StudentController(mockScheduleService.Object, mockUsersService.Object);

        // Act
        var result = await controller.GetAnonymousSchedule(request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).Value.Should().BeOfType<StudentScheduleResponse>();
        var response = (StudentScheduleResponse)((OkObjectResult)result).Value!;
        response.Schedule.Should().NotBeNull();
        response.Schedule.Should().HaveCount(1);
    }
    
    [Fact]
    public async Task StudentController_GetPersonalSchedule_ReturnsOk()
    {
        // Arrange
        var mockScheduleService = new Mock<IStudentService>();
        var mockUsersService = new Mock<IUsersService>();
        mockUsersService.Setup(x => x.GetUserAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserEntity
            {
                Id = Guid.NewGuid(),
                FullName = "Test user",
                Group = "Test group",
                IsRestricted = false
            });
        mockScheduleService.Setup(x => x.GetScheduleAsync(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<UserEntity>()))
            .ReturnsAsync(new List<StudentLesson>
            {
                new() { Id = Guid.NewGuid(), Discipline = "Test subject" }
            });
        var request = new StudentScheduleRequest
        {
            Date = DateTime.Now.ToString("yyyy-MM-dd"),
            StudyGroup = "test",
            OutageGroup = "test"
        };
        var controller = new StudentController(mockScheduleService.Object, mockUsersService.Object);

        // Act
        var result = await controller.GetPersonalSchedule(request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).Value.Should().BeOfType<StudentScheduleResponse>();
        var response = (StudentScheduleResponse)((OkObjectResult)result).Value!;
        response.Schedule.Should().NotBeNull();
        response.Schedule.Should().HaveCount(1);
    }
    
    [Fact]
    public async Task StudentController_GetPersonalSchedule_ReturnsForbid()
    {
        // Arrange
        var mockScheduleService = new Mock<IStudentService>();
        var mockUsersService = new Mock<IUsersService>();
        mockUsersService.Setup(x => x.GetUserAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as UserEntity);
        mockScheduleService.Setup(x => x.GetScheduleAsync(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<UserEntity>()))
            .ReturnsAsync(new List<StudentLesson>()).Verifiable();
        var request = new StudentScheduleRequest
        {
            Date = DateTime.Now.ToString("yyyy-MM-dd"),
            StudyGroup = "test",
            OutageGroup = "test"
        };
        var controller = new StudentController(mockScheduleService.Object, mockUsersService.Object);

        // Act
        var result = await controller.GetPersonalSchedule(request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ForbidResult>();
        mockScheduleService.Verify(x => x.GetScheduleAsync(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<UserEntity>()), Times.Never);
    }
    
    [Fact]
    public async Task StudentController_GetFilters_ReturnsOk()
    {
        // Arrange
        var mockStudentService = new Mock<IStudentService>();
        var mockUsersService = new Mock<IUsersService>();
        mockStudentService.Setup(x => x.GetFiltersAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dictionary<string, List<KeyValuePair<string, string>>>()
            {
                { "courses", new List<KeyValuePair<string, string>>() },
                { "educForms", new List<KeyValuePair<string, string>>() },
                { "faculties", new List<KeyValuePair<string, string>>() },
            });
        var controller = new StudentController(mockStudentService.Object, mockUsersService.Object);

        // Act
        var result = await controller.GetFilters(CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).Value.Should().BeOfType<StudentFiltersResponse>();
        var response = (StudentFiltersResponse)((OkObjectResult)result).Value!;
        response.Filters.Should().NotBeNull();
        response.Filters.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task StudentController_GetStudyGroups_ReturnsOk()
    {
        // Arrange
        var mockStudentService = new Mock<IStudentService>();
        var mockUsersService = new Mock<IUsersService>();
        mockStudentService.Setup(x => x.GetStudyGroupsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<KeyValuePair<string, string>>()
            {
                new("test1", "1"),
                new("test2", "2"),
                new("test3", "3"),
            });
        var controller = new StudentController(mockStudentService.Object, mockUsersService.Object);
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
        ((OkObjectResult)result).Value.Should().BeOfType<FiltersResponse>();
        var response = (FiltersResponse)((OkObjectResult)result).Value!;
        response.Filters.Should().NotBeNull();
        response.Filters.Should().HaveCount(3);
    }
}