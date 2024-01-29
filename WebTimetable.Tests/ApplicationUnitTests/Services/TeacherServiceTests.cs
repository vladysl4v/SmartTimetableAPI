using WebTimetable.Application.Entities;
using WebTimetable.Application.Handlers.Abstractions;
using WebTimetable.Application.Models;
using WebTimetable.Application.Services;

namespace WebTimetable.Tests.ApplicationUnitTests.Services;

public class TeacherServiceTests
{
    [Fact]
    public async Task TeacherService_GetScheduleAsync_ReturnScheduleWithOutages()
    {
        // Arrange
        var mockScheduleHandler = new Mock<IRequestHandler>();
        mockScheduleHandler.Setup(x => x.GetTeacherSchedule(It.IsAny<DateTime>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new List<TeacherLesson> { new(), new(), new() });
        
        var mockOutagesHandler = new Mock<IOutagesHandler>();
        mockOutagesHandler.Setup(x => x.ConfigureOutagesAsync(It.IsAny<IEnumerable<TeacherLesson>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Verifiable();
        var mockEventsHandler = new Mock<IEventsHandler>();
        
        var scheduleService = new TeacherService(mockEventsHandler.Object, mockScheduleHandler.Object,
            mockOutagesHandler.Object);
        
        // Act
        var lessons =
            await scheduleService.GetScheduleAsync(DateTime.Now, "Test", "Group 1", CancellationToken.None);
        
        // Assert
        mockOutagesHandler.Verify(mr => mr.ConfigureOutagesAsync(It.IsAny<IEnumerable<TeacherLesson>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
        lessons.Should().NotBeNull();
        lessons.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task TeacherService_GetScheduleAsync_ReturnScheduleWithoutOutages()
    {
        // Arrange
        var mockScheduleHandler = new Mock<IRequestHandler>();
        mockScheduleHandler.Setup(x => x.GetTeacherSchedule(It.IsAny<DateTime>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new List<TeacherLesson> { new(), new(), new() });
        
        var mockOutagesHandler = new Mock<IOutagesHandler>();
        mockOutagesHandler.Setup(x => x.ConfigureOutagesAsync(It.IsAny<IEnumerable<TeacherLesson>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Verifiable();
        var mockEventsHandler = new Mock<IEventsHandler>();
        
        var scheduleService = new TeacherService(mockEventsHandler.Object, mockScheduleHandler.Object,
            mockOutagesHandler.Object);
        
        // Act
        var lessons =
            await scheduleService.GetScheduleAsync(DateTime.Now, "Test", "", CancellationToken.None);
        
        // Assert
        mockOutagesHandler.Verify(mr => mr.ConfigureOutagesAsync(It.IsAny<IEnumerable<TeacherLesson>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never());
        lessons.Should().NotBeNull();
        lessons.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task TeacherService_GetScheduleAsync_ReturnScheduleWithOutages_Authorized()
    {
        // Arrange
        var mockScheduleHandler = new Mock<IRequestHandler>();
        
        mockScheduleHandler.Setup(x => x.GetTeacherSchedule(It.IsAny<DateTime>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new List<TeacherLesson> { new(), new(), new() });
        
        var mockOutagesHandler = new Mock<IOutagesHandler>();
        mockOutagesHandler.Setup(x => x.ConfigureOutagesAsync(It.IsAny<IEnumerable<TeacherLesson>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Verifiable();
        
        var mockEventsHandler = new Mock<IEventsHandler>();
        mockEventsHandler.Setup(x => x.ConfigureEventsAsync(It.IsAny<List<TeacherLesson>>(), It.IsAny<CancellationToken>())).Verifiable();
        
        var scheduleService = new TeacherService(mockEventsHandler.Object, mockScheduleHandler.Object,
            mockOutagesHandler.Object);
        
        // Act
        var lessons = await scheduleService.GetScheduleAsync(DateTime.Now, "Test", "Group 1", CancellationToken.None, new UserEntity());

        // Assert
        mockOutagesHandler.Verify(x => x.ConfigureOutagesAsync(It.IsAny<IEnumerable<TeacherLesson>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
        mockEventsHandler.Verify(x => x.ConfigureEventsAsync(It.IsAny<List<TeacherLesson>>(), It.IsAny<CancellationToken>()), Times.Once());
        lessons.Should().NotBeNull();
        lessons.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task TeacherService_GetScheduleAsync_ReturnScheduleWithoutOutages_Authorized()
    {
        // Arrange
        var mockScheduleHandler = new Mock<IRequestHandler>();
        
        mockScheduleHandler.Setup(x => x.GetTeacherSchedule(It.IsAny<DateTime>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new List<TeacherLesson> { new(), new(), new() });
        
        var mockOutagesHandler = new Mock<IOutagesHandler>();
        mockOutagesHandler.Setup(x => x.ConfigureOutagesAsync(It.IsAny<IEnumerable<TeacherLesson>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Verifiable();
        
        var mockEventsHandler = new Mock<IEventsHandler>();
        mockEventsHandler.Setup(x => x.ConfigureEventsAsync(It.IsAny<List<TeacherLesson>>(), It.IsAny<CancellationToken>())).Verifiable();

        
        var scheduleService = new TeacherService(mockEventsHandler.Object, mockScheduleHandler.Object,
            mockOutagesHandler.Object);
        
        // Act
        var lessons = await scheduleService.GetScheduleAsync(DateTime.Now, "Test", "", CancellationToken.None, new UserEntity());

        // Assert
        mockOutagesHandler.Verify(x => x.ConfigureOutagesAsync(It.IsAny<IEnumerable<TeacherLesson>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never());
        mockEventsHandler.Verify(x => x.ConfigureEventsAsync(It.IsAny<List<TeacherLesson>>(), It.IsAny<CancellationToken>()), Times.Once());
        lessons.Should().NotBeNull();
        lessons.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task TeacherService_GetFacultiesAsync_ReturnFaculties()
    {
        // Arrange
        var mockScheduleHandler = new Mock<IRequestHandler>();
        mockScheduleHandler.Setup(x => x.GetTeacherFaculties(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<KeyValuePair<string, string>> { new(), new(), new() });
        
        var mockOutagesHandler = new Mock<IOutagesHandler>();
        var mockEventsHandler = new Mock<IEventsHandler>();
        
        var scheduleService = new TeacherService(mockEventsHandler.Object, mockScheduleHandler.Object,
            mockOutagesHandler.Object);
        
        // Act
        var faculties = await scheduleService.GetFacultiesAsync(CancellationToken.None);
        
        // Assert
        faculties.Should().NotBeNull();
        faculties.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task TeacherService_GetChairsAsync_ReturnChairs()
    {
        // Arrange
        var mockScheduleHandler = new Mock<IRequestHandler>();
        mockScheduleHandler.Setup(x => x.GetTeacherChairs(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<KeyValuePair<string, string>> { new(), new(), new() });
        
        var mockOutagesHandler = new Mock<IOutagesHandler>();
        var mockEventsHandler = new Mock<IEventsHandler>();
        
        var scheduleService = new TeacherService(mockEventsHandler.Object, mockScheduleHandler.Object,
            mockOutagesHandler.Object);
        
        // Act
        var chairs = await scheduleService.GetChairsAsync("NNN", CancellationToken.None);
        
        // Assert
        chairs.Should().NotBeNull();
        chairs.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task TeacherService_GetEmployeesAsync_ReturnEmployees()
    {
        // Arrange
        var mockScheduleHandler = new Mock<IRequestHandler>();
        mockScheduleHandler.Setup(x => x.GetTeacherEmployees(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<KeyValuePair<string, string>> { new(), new(), new() });
        
        var mockOutagesHandler = new Mock<IOutagesHandler>();
        var mockEventsHandler = new Mock<IEventsHandler>();
        
        var scheduleService = new TeacherService(mockEventsHandler.Object, mockScheduleHandler.Object,
            mockOutagesHandler.Object);
        
        // Act
        var employees = await scheduleService.GetEmployeesAsync("NNN", "FYP", CancellationToken.None);
        
        // Assert
        employees.Should().NotBeNull();
        employees.Should().HaveCount(3);
    }
}