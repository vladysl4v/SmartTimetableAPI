using Microsoft.AspNetCore.Mvc;
using WebTimetable.Api.Controllers;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Models;
using WebTimetable.Application.Services.Abstractions;
using WebTimetable.Contracts.Requests;
using WebTimetable.Contracts.Responses;

namespace WebTimetable.Tests.ApiUnitTests.Controllers;

public class ScheduleControllerTests
{
    [Fact]
    public async Task ScheduleController_GetAnonymousSchedule_ReturnsOk()
    {
        // Arrange
        var mockScheduleService = new Mock<IScheduleService>();
        var mockUsersService = new Mock<IUsersService>();
        mockScheduleService.Setup(x => x.GetGuestSchedule(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Lesson>
            {
                new() { Id = Guid.NewGuid(), Discipline = "Test subject" }
            });
        var request = new AnonymousScheduleRequest
        {
            Date = DateTime.Now.ToString("yyyy-MM-dd"),
            StudyGroup = "test",
            OutageGroup = "test"
        };
        var controller = new ScheduleController(mockScheduleService.Object, mockUsersService.Object);

        // Act
        var result = await controller.GetAnonymousSchedule(request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).Value.Should().BeOfType<ScheduleResponse>();
        var response = (ScheduleResponse)((OkObjectResult)result).Value!;
        response.Schedule.Should().NotBeNull();
        response.Schedule.Should().HaveCount(1);
    }
    
    [Fact]
    public async Task ScheduleController_GetPersonalSchedule_ReturnsOk()
    {
        // Arrange
        var mockScheduleService = new Mock<IScheduleService>();
        var mockUsersService = new Mock<IUsersService>();
        mockUsersService.Setup(x => x.GetUser(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserEntity
            {
                Id = Guid.NewGuid(),
                FullName = "Test user",
                Group = "Test group",
                IsRestricted = false
            });
        mockScheduleService.Setup(x => x.GetPersonalSchedule(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Lesson>
            {
                new() { Id = Guid.NewGuid(), Discipline = "Test subject" }
            });
        var request = new PersonalScheduleRequest
        {
            Date = DateTime.Now.ToString("yyyy-MM-dd"),
            StudyGroup = "test",
            OutageGroup = "test"
        };
        var controller = new ScheduleController(mockScheduleService.Object, mockUsersService.Object);

        // Act
        var result = await controller.GetPersonalSchedule(request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).Value.Should().BeOfType<ScheduleResponse>();
        var response = (ScheduleResponse)((OkObjectResult)result).Value!;
        response.Schedule.Should().NotBeNull();
        response.Schedule.Should().HaveCount(1);
    }
    
    [Fact]
    public async Task ScheduleController_GetPersonalSchedule_ReturnsForbid()
    {
        // Arrange
        var mockScheduleService = new Mock<IScheduleService>();
        var mockUsersService = new Mock<IUsersService>();
        mockUsersService.Setup(x => x.GetUser(It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as UserEntity);
        mockScheduleService.Setup(x => x.GetPersonalSchedule(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as List<Lesson>);
        var request = new PersonalScheduleRequest
        {
            Date = DateTime.Now.ToString("yyyy-MM-dd"),
            StudyGroup = "test",
            OutageGroup = "test"
        };
        var controller = new ScheduleController(mockScheduleService.Object, mockUsersService.Object);

        // Act
        var result = await controller.GetPersonalSchedule(request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ForbidResult>();
    }
}