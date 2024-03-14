using Microsoft.AspNetCore.Mvc;
using WebTimetable.Api.Controllers;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Models;
using WebTimetable.Application.Services.Abstractions;
using WebTimetable.Contracts.Requests;
using WebTimetable.Contracts.Responses;

namespace WebTimetable.Tests.ApiUnitTests.Controllers;

public class TeachersControllerTests
{
    [Fact]
    public async Task TeacherController_GetSchedule_ReturnsOk()
    {
        // Arrange
        var mockScheduleService = new Mock<ITeacherService>();
        var mockUsersService = new Mock<IUsersService>();
        mockScheduleService.Setup(x => x.GetScheduleAsync(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<TeacherLesson>
            {
                new() { Id = Guid.NewGuid(), Discipline = "Test subject" }
            });
        var request = new ScheduleRequest
        {
            Date = DateTime.Now.ToString("yyyy-MM-dd"),
            Identifier = "test"
        };
        var controller = new TeachersController(mockScheduleService.Object, mockUsersService.Object);

        // Act
        var result = await controller.GetSchedule(request, CancellationToken.None, "test");

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).Value.Should().BeOfType<TeacherScheduleResponse>();
        var response = (TeacherScheduleResponse)((OkObjectResult)result).Value!;
        response.Schedule.Should().NotBeNull();
        response.Schedule.Should().HaveCount(1);
    }
    
    [Fact]
    public async Task TeacherController_GetLessonDetails_ReturnsOk()
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
        mockScheduleService.Setup(x => x.GetLessonDetails(It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<TimeOnly>(),
                It.IsAny<TimeOnly>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new LessonDetails
            {
                Id = Guid.NewGuid(),
                Notes = new List<NoteEntity>(),
                Events = new List<Event>()
            });
        var request = new LessonDetailsRequest
        {
            Date = DateTime.Now.ToString("yyyy-MM-dd"),
            StartTime = "08:00",
            EndTime = "09:30"
        };

        var controller = new TeachersController(mockScheduleService.Object, mockUsersService.Object);

        // Act
        var result = await controller.GetLessonDetails(Guid.NewGuid(), request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).Value.Should().BeOfType<LessonDetailsResponse>();
        var response = (LessonDetailsResponse)((OkObjectResult)result).Value!;
        response.Notes.Should().BeEmpty();
        response.Meetings.Should().BeEmpty();
    }
    
    [Fact]
    public async Task TeacherController_GetIndividualSchedule_ReturnsForbid()
    {
        // Arrange
        var mockScheduleService = new Mock<ITeacherService>();
        var mockUsersService = new Mock<IUsersService>();
        mockUsersService.Setup(x => x.GetUserAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as UserEntity);
        mockScheduleService.Setup(x => x.GetLessonDetails(It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<TimeOnly>(),
                It.IsAny<TimeOnly>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new LessonDetails()).Verifiable();
        var request = new LessonDetailsRequest
        {
            Date = DateTime.Now.ToString("yyyy-MM-dd"),
            StartTime = "08:00",
            EndTime = "09:30"
        };

        var controller = new TeachersController(mockScheduleService.Object, mockUsersService.Object);

        // Act
        var result = await controller.GetLessonDetails(Guid.NewGuid(), request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ForbidResult>();
        mockScheduleService.Verify(x => x.GetLessonDetails(It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<TimeOnly>(),
            It.IsAny<TimeOnly>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task TeacherController_GetFaculties_ReturnsOk()
    {
        // Arrange
        var mockTeacherService = new Mock<ITeacherService>();
        var mockUsersService = new Mock<IUsersService>();
        mockTeacherService.Setup(x => x.GetFacultiesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new  List<KeyValuePair<string, string>> { new(), new(), new(), });
        var controller = new TeachersController(mockTeacherService.Object, mockUsersService.Object);

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
        var controller = new TeachersController(mockTeacherService.Object, mockUsersService.Object);

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
        var controller = new TeachersController(mockTeacherService.Object, mockUsersService.Object);

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