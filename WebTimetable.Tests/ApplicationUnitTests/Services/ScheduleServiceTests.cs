using WebTimetable.Application.Entities;
using WebTimetable.Application.Handlers.Abstractions;
using WebTimetable.Application.Models;
using WebTimetable.Application.Services;

namespace WebTimetable.Tests.ApplicationUnitTests.Services;

public class ScheduleServiceTests
{
    [Fact]
    public async Task ScheduleService_GetGuestSchedule_ReturnScheduleWithOutages()
    {
        // Arrange
        var mockScheduleHandler = new Mock<IScheduleHandler>();
        mockScheduleHandler.Setup(x => x.GetSchedule(It.IsAny<DateTime>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new List<Lesson> { new(), new(), new() });
        
        var mockOutagesHandler = new Mock<IOutagesHandler>();
        mockOutagesHandler.Setup(x => x.ConfigureOutagesAsync(It.IsAny<IEnumerable<Lesson>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Verifiable();
        var mockEventsHandler = new Mock<IEventsHandler>();
        var mockNotesHandler = new Mock<INotesHandler>();
        
        var scheduleService = new ScheduleService(mockEventsHandler.Object, mockScheduleHandler.Object,
            mockOutagesHandler.Object, mockNotesHandler.Object);
        
        // Act
        var lessons =
            await scheduleService.GetGuestSchedule(DateTime.Now, "Test", "Group 1", CancellationToken.None);
        
        // Assert
        mockOutagesHandler.Verify(mr => mr.ConfigureOutagesAsync(It.IsAny<IEnumerable<Lesson>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
        lessons.Should().NotBeNull();
        lessons.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task ScheduleService_GetGuestSchedule_ReturnScheduleWithoutOutages()
    {
        // Arrange
        var mockScheduleHandler = new Mock<IScheduleHandler>();
        mockScheduleHandler.Setup(x => x.GetSchedule(It.IsAny<DateTime>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new List<Lesson> { new(), new(), new() });
        
        var mockOutagesHandler = new Mock<IOutagesHandler>();
        mockOutagesHandler.Setup(x => x.ConfigureOutagesAsync(It.IsAny<IEnumerable<Lesson>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Verifiable();
        var mockEventsHandler = new Mock<IEventsHandler>();
        var mockNotesHandler = new Mock<INotesHandler>();
        
        var scheduleService = new ScheduleService(mockEventsHandler.Object, mockScheduleHandler.Object,
            mockOutagesHandler.Object, mockNotesHandler.Object);
        
        // Act
        var lessons =
            await scheduleService.GetGuestSchedule(DateTime.Now, "Test", "", CancellationToken.None);
        
        // Assert
        mockOutagesHandler.Verify(mr => mr.ConfigureOutagesAsync(It.IsAny<IEnumerable<Lesson>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never());
        lessons.Should().NotBeNull();
        lessons.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task ScheduleService_GetPersonalSchedule_ReturnScheduleWithOutages()
    {
        // Arrange
        var mockScheduleHandler = new Mock<IScheduleHandler>();
        
        mockScheduleHandler.Setup(x => x.GetSchedule(It.IsAny<DateTime>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new List<Lesson> { new(), new(), new() });
        
        var mockOutagesHandler = new Mock<IOutagesHandler>();
        mockOutagesHandler.Setup(x => x.ConfigureOutagesAsync(It.IsAny<IEnumerable<Lesson>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Verifiable();
        
        var mockEventsHandler = new Mock<IEventsHandler>();
        mockEventsHandler.Setup(x => x.ConfigureEvents(It.IsAny<List<Lesson>>())).Verifiable();

        var mockNotesHandler = new Mock<INotesHandler>();
        mockNotesHandler.Setup(x => x.ConfigureNotes(It.IsAny<List<Lesson>>(), It.IsAny<string>(), It.IsAny<Guid>())).Verifiable();
        
        var scheduleService = new ScheduleService(mockEventsHandler.Object, mockScheduleHandler.Object,
            mockOutagesHandler.Object, mockNotesHandler.Object);
        
        // Act
        var lessons = await scheduleService.GetPersonalSchedule(DateTime.Now, "Test", "Group 1", new UserEntity(),
            CancellationToken.None);

        // Assert
        mockOutagesHandler.Verify(x => x.ConfigureOutagesAsync(It.IsAny<IEnumerable<Lesson>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
        mockEventsHandler.Verify(x => x.ConfigureEvents(It.IsAny<List<Lesson>>()), Times.Once());
        mockNotesHandler.Verify(x => x.ConfigureNotes(It.IsAny<List<Lesson>>(), It.IsAny<string>(), It.IsAny<Guid>()), Times.Once());
        lessons.Should().NotBeNull();
        lessons.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task ScheduleService_GetPersonalSchedule_ReturnScheduleWithoutOutages()
    {
        // Arrange
        var mockScheduleHandler = new Mock<IScheduleHandler>();
        
        mockScheduleHandler.Setup(x => x.GetSchedule(It.IsAny<DateTime>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new List<Lesson> { new(), new(), new() });
        
        var mockOutagesHandler = new Mock<IOutagesHandler>();
        mockOutagesHandler.Setup(x => x.ConfigureOutagesAsync(It.IsAny<IEnumerable<Lesson>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Verifiable();
        
        var mockEventsHandler = new Mock<IEventsHandler>();
        mockEventsHandler.Setup(x => x.ConfigureEvents(It.IsAny<List<Lesson>>())).Verifiable();

        var mockNotesHandler = new Mock<INotesHandler>();
        mockNotesHandler.Setup(x => x.ConfigureNotes(It.IsAny<List<Lesson>>(), It.IsAny<string>(), It.IsAny<Guid>())).Verifiable();
        
        var scheduleService = new ScheduleService(mockEventsHandler.Object, mockScheduleHandler.Object,
            mockOutagesHandler.Object, mockNotesHandler.Object);
        
        // Act
        var lessons = await scheduleService.GetPersonalSchedule(DateTime.Now, "Test", "", new UserEntity(),
            CancellationToken.None);

        // Assert
        mockOutagesHandler.Verify(x => x.ConfigureOutagesAsync(It.IsAny<IEnumerable<Lesson>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never());
        mockEventsHandler.Verify(x => x.ConfigureEvents(It.IsAny<List<Lesson>>()), Times.Once());
        mockNotesHandler.Verify(x => x.ConfigureNotes(It.IsAny<List<Lesson>>(), It.IsAny<string>(), It.IsAny<Guid>()), Times.Once());
        lessons.Should().NotBeNull();
        lessons.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task ScheduleService_GetPersonalSchedule_ReturnNull()
    {
        // Arrange
        UserEntity user = null;
        var mockScheduleHandler = new Mock<IScheduleHandler>();
        var mockOutagesHandler = new Mock<IOutagesHandler>();
        var mockEventsHandler = new Mock<IEventsHandler>();
        var mockNotesHandler = new Mock<INotesHandler>();
        var scheduleService = new ScheduleService(mockEventsHandler.Object, mockScheduleHandler.Object,
            mockOutagesHandler.Object, mockNotesHandler.Object);
        
        // Act
        var lessons =
            await scheduleService.GetPersonalSchedule(DateTime.Now, "Test", "Group 1", user,
                CancellationToken.None);

        // Assert
        lessons.Should().BeNull();
    }
}