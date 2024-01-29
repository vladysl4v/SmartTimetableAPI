using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models;
using WebTimetable.Api.Controllers;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Models;
using WebTimetable.Application.Services.Abstractions;
using WebTimetable.Contracts.Requests;
using WebTimetable.Contracts.Responses;

namespace WebTimetable.Tests.ApiUnitTests.Controllers;

public class TeacherControllerTests
{
    [Fact]
    public async Task TeacherController_GetAnonymousSchedule_ReturnsOk()
    {
        // Arrange
        var mockScheduleService = new Mock<ITeacherService>();
        var mockUsersService = new Mock<IUsersService>();
        mockScheduleService.Setup(x => x.GetScheduleAsync(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<UserEntity>()))
            .ReturnsAsync(new List<TeacherLesson>
            {
                new() { Id = Guid.NewGuid(), Discipline = "Test subject" }
            });
        var request = new TeacherScheduleRequest
        {
            Date = DateTime.Now.ToString("yyyy-MM-dd"),
            TeacherId = "test",
            OutageGroup = "test"
        };
        var controller = new TeacherController(mockScheduleService.Object, mockUsersService.Object);

        // Act
        var result = await controller.GetAnonymousSchedule(request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).Value.Should().BeOfType<TeacherScheduleResponse>();
        var response = (TeacherScheduleResponse)((OkObjectResult)result).Value!;
        response.Schedule.Should().NotBeNull();
        response.Schedule.Should().HaveCount(1);
    }
    
    [Fact]
    public async Task TeacherController_GetPersonalSchedule_ReturnsOk()
    {
        // Arrange
        var mockScheduleService = new Mock<ITeacherService>();
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
            .ReturnsAsync(new List<TeacherLesson>
            {
                new() { Id = Guid.NewGuid(), Discipline = "Test subject" }
            });
        var request = new TeacherScheduleRequest
        {
            Date = DateTime.Now.ToString("yyyy-MM-dd"),
            TeacherId = "test",
            OutageGroup = "test"
        };
        var controller = new TeacherController(mockScheduleService.Object, mockUsersService.Object);

        // Act
        var result = await controller.GetPersonalSchedule(request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).Value.Should().BeOfType<TeacherScheduleResponse>();
        var response = (TeacherScheduleResponse)((OkObjectResult)result).Value!;
        response.Schedule.Should().NotBeNull();
        response.Schedule.Should().HaveCount(1);
    }
    
    [Fact]
    public async Task TeacherController_GetPersonalSchedule_ReturnsForbid()
    {
        // Arrange
        var mockScheduleService = new Mock<ITeacherService>();
        var mockUsersService = new Mock<IUsersService>();
        mockUsersService.Setup(x => x.GetUserAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as UserEntity);
        mockScheduleService.Setup(x => x.GetScheduleAsync(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<UserEntity>()))
            .ReturnsAsync(new List<TeacherLesson>()).Verifiable();
        var request = new TeacherScheduleRequest
        {
            Date = DateTime.Now.ToString("yyyy-MM-dd"),
            TeacherId = "test",
            OutageGroup = "test"
        };
        var controller = new TeacherController(mockScheduleService.Object, mockUsersService.Object);

        // Act
        var result = await controller.GetPersonalSchedule(request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ForbidResult>();
        mockScheduleService.Verify(x => x.GetScheduleAsync(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>(), It.IsAny<UserEntity>()), Times.Never);
    }
    
    [Fact]
    public async Task TeacherController_GetFaculties_ReturnsOk()
    {
        // Arrange
        var mockTeacherService = new Mock<ITeacherService>();
        var mockUsersService = new Mock<IUsersService>();
        mockTeacherService.Setup(x => x.GetFacultiesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new  List<KeyValuePair<string, string>> { new(), new(), new(), });
        var controller = new TeacherController(mockTeacherService.Object, mockUsersService.Object);

        // Act
        var result = await controller.GetFaculties(CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).Value.Should().BeOfType<FiltersResponse>();
        var response = (FiltersResponse)((OkObjectResult)result).Value!;
        response.Filters.Should().NotBeNull();
        response.Filters.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task TeacherController_GetChairs_ReturnsOk()
    {
        // Arrange
        var mockTeacherService = new Mock<ITeacherService>();
        var mockUsersService = new Mock<IUsersService>();
        mockTeacherService.Setup(x => x.GetChairsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new  List<KeyValuePair<string, string>> { new(), new(), new(), });
        var controller = new TeacherController(mockTeacherService.Object, mockUsersService.Object);

        // Act
        var request = new ChairsRequest
        {
            Faculty = "NNN"
        };
        var result = await controller.GetChairs(request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).Value.Should().BeOfType<FiltersResponse>();
        var response = (FiltersResponse)((OkObjectResult)result).Value!;
        response.Filters.Should().NotBeNull();
        response.Filters.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task TeacherController_GetEmployees_ReturnsOk()
    {
        // Arrange
        var mockTeacherService = new Mock<ITeacherService>();
        var mockUsersService = new Mock<IUsersService>();
        mockTeacherService.Setup(x => x.GetEmployeesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new  List<KeyValuePair<string, string>> { new(), new(), new(), });
        var controller = new TeacherController(mockTeacherService.Object, mockUsersService.Object);

        // Act
        var request = new EmployeesRequest
        {
            Faculty = "NNN",
            Chair = "FYP"
        };
        var result = await controller.GetEmployees(request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).Value.Should().BeOfType<FiltersResponse>();
        var response = (FiltersResponse)((OkObjectResult)result).Value!;
        response.Filters.Should().NotBeNull();
        response.Filters.Should().HaveCount(3);
    }
    
}