using WebTimetable.Application.Entities;
using WebTimetable.Application.Handlers.Abstractions;
using WebTimetable.Application.Models;
using WebTimetable.Application.Services;

namespace WebTimetable.Tests.ApplicationUnitTests.Services;

public class StudentServiceTests
{
    [Fact]
    public async Task StudentService_GetScheduleAsync_ReturnScheduleWithOutages()
    {
        // Arrange
        var mockScheduleHandler = new Mock<IRequestHandler>();
        mockScheduleHandler.Setup(x => x.GetStudentSchedule(It.IsAny<DateTime>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new List<StudentLesson> { new(), new(), new() });
        
        var mockOutagesHandler = new Mock<IOutagesHandler>();
        mockOutagesHandler.Setup(x => x.ConfigureOutagesAsync(It.IsAny<IEnumerable<StudentLesson>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Verifiable();
        var mockEventsHandler = new Mock<IEventsHandler>();
        var mockNotesHandler = new Mock<INotesHandler>();
        
        var scheduleService = new StudentService(mockEventsHandler.Object, mockScheduleHandler.Object,
            mockOutagesHandler.Object, mockNotesHandler.Object);
        
        // Act
        var lessons =
            await scheduleService.GetScheduleAsync(DateTime.Now, "Test", "Group 1", CancellationToken.None);
        
        // Assert
        mockOutagesHandler.Verify(mr => mr.ConfigureOutagesAsync(It.IsAny<IEnumerable<StudentLesson>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
        lessons.Should().NotBeNull();
        lessons.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task StudentService_GetScheduleAsync_ReturnScheduleWithoutOutages()
    {
        // Arrange
        var mockScheduleHandler = new Mock<IRequestHandler>();
        mockScheduleHandler.Setup(x => x.GetStudentSchedule(It.IsAny<DateTime>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new List<StudentLesson> { new(), new(), new() });
        
        var mockOutagesHandler = new Mock<IOutagesHandler>();
        mockOutagesHandler.Setup(x => x.ConfigureOutagesAsync(It.IsAny<IEnumerable<StudentLesson>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Verifiable();
        var mockEventsHandler = new Mock<IEventsHandler>();
        var mockNotesHandler = new Mock<INotesHandler>();
        
        var scheduleService = new StudentService(mockEventsHandler.Object, mockScheduleHandler.Object,
            mockOutagesHandler.Object, mockNotesHandler.Object);
        
        // Act
        var lessons =
            await scheduleService.GetScheduleAsync(DateTime.Now, "Test", "", CancellationToken.None);
        
        // Assert
        mockOutagesHandler.Verify(mr => mr.ConfigureOutagesAsync(It.IsAny<IEnumerable<StudentLesson>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never());
        lessons.Should().NotBeNull();
        lessons.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task StudentService_GetScheduleAsync_ReturnScheduleWithOutages_Authorized()
    {
        // Arrange
        var mockScheduleHandler = new Mock<IRequestHandler>();
        
        mockScheduleHandler.Setup(x => x.GetStudentSchedule(It.IsAny<DateTime>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new List<StudentLesson> { new(), new(), new() });
        
        var mockOutagesHandler = new Mock<IOutagesHandler>();
        mockOutagesHandler.Setup(x => x.ConfigureOutagesAsync(It.IsAny<IEnumerable<StudentLesson>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Verifiable();
        
        var mockEventsHandler = new Mock<IEventsHandler>();
        mockEventsHandler.Setup(x => x.ConfigureEventsAsync(It.IsAny<List<StudentLesson>>(), It.IsAny<CancellationToken>())).Verifiable();

        var mockNotesHandler = new Mock<INotesHandler>();
        mockNotesHandler.Setup(x => x.ConfigureNotes(It.IsAny<List<StudentLesson>>(), It.IsAny<string>(), It.IsAny<Guid>())).Verifiable();
        
        var scheduleService = new StudentService(mockEventsHandler.Object, mockScheduleHandler.Object,
            mockOutagesHandler.Object, mockNotesHandler.Object);
        
        // Act
        var lessons = await scheduleService.GetScheduleAsync(DateTime.Now, "Test", "Group 1", CancellationToken.None, new UserEntity());

        // Assert
        mockOutagesHandler.Verify(x => x.ConfigureOutagesAsync(It.IsAny<IEnumerable<StudentLesson>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
        mockEventsHandler.Verify(x => x.ConfigureEventsAsync(It.IsAny<List<StudentLesson>>(), It.IsAny<CancellationToken>()), Times.Once());
        mockNotesHandler.Verify(x => x.ConfigureNotes(It.IsAny<List<StudentLesson>>(), It.IsAny<string>(), It.IsAny<Guid>()), Times.Once());
        lessons.Should().NotBeNull();
        lessons.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task StudentService_GetScheduleAsync_ReturnScheduleWithoutOutages_Authorized()
    {
        // Arrange
        var mockScheduleHandler = new Mock<IRequestHandler>();
        
        mockScheduleHandler.Setup(x => x.GetStudentSchedule(It.IsAny<DateTime>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new List<StudentLesson> { new(), new(), new() });
        
        var mockOutagesHandler = new Mock<IOutagesHandler>();
        mockOutagesHandler.Setup(x => x.ConfigureOutagesAsync(It.IsAny<IEnumerable<StudentLesson>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Verifiable();
        
        var mockEventsHandler = new Mock<IEventsHandler>();
        mockEventsHandler.Setup(x => x.ConfigureEventsAsync(It.IsAny<List<StudentLesson>>(), It.IsAny<CancellationToken>())).Verifiable();

        var mockNotesHandler = new Mock<INotesHandler>();
        mockNotesHandler.Setup(x => x.ConfigureNotes(It.IsAny<List<StudentLesson>>(), It.IsAny<string>(), It.IsAny<Guid>())).Verifiable();
        
        var scheduleService = new StudentService(mockEventsHandler.Object, mockScheduleHandler.Object,
            mockOutagesHandler.Object, mockNotesHandler.Object);
        
        // Act
        var lessons = await scheduleService.GetScheduleAsync(DateTime.Now, "Test", "", CancellationToken.None, new UserEntity());

        // Assert
        mockOutagesHandler.Verify(x => x.ConfigureOutagesAsync(It.IsAny<IEnumerable<StudentLesson>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never());
        mockEventsHandler.Verify(x => x.ConfigureEventsAsync(It.IsAny<List<StudentLesson>>(), It.IsAny<CancellationToken>()), Times.Once());
        mockNotesHandler.Verify(x => x.ConfigureNotes(It.IsAny<List<StudentLesson>>(), It.IsAny<string>(), It.IsAny<Guid>()), Times.Once());
        lessons.Should().NotBeNull();
        lessons.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task StudentService_GetFiltersAsync_ReturnFilters()
    {
        // Arrange
        var mockScheduleHandler = new Mock<IRequestHandler>();
        mockScheduleHandler.Setup(x => x.GetStudentFilters(It.IsAny<CancellationToken>())).ReturnsAsync(new Dictionary<string, List<KeyValuePair<string, string>>>());
        
        var mockOutagesHandler = new Mock<IOutagesHandler>();
        var mockEventsHandler = new Mock<IEventsHandler>();
        var mockNotesHandler = new Mock<INotesHandler>();
        
        var scheduleService = new StudentService(mockEventsHandler.Object, mockScheduleHandler.Object,
            mockOutagesHandler.Object, mockNotesHandler.Object);
        
        // Act
        var filters = await scheduleService.GetFiltersAsync(CancellationToken.None);
        
        // Assert
        filters.Should().NotBeNull();
    }
    
    [Fact]
    public async Task StudentService_GetStudyGroupsAsync_ReturnStudyGroups()
    {
        // Arrange
        var mockScheduleHandler = new Mock<IRequestHandler>();
        mockScheduleHandler.Setup(x => x.GetStudentStudyGroups(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<KeyValuePair<string, string>>());
        
        var mockOutagesHandler = new Mock<IOutagesHandler>();
        var mockEventsHandler = new Mock<IEventsHandler>();
        var mockNotesHandler = new Mock<INotesHandler>();
        
        var scheduleService = new StudentService(mockEventsHandler.Object, mockScheduleHandler.Object,
            mockOutagesHandler.Object, mockNotesHandler.Object);
        
        // Act
        var studyGroups = await scheduleService.GetStudyGroupsAsync("Test", 1, 1, CancellationToken.None);
        
        // Assert
        studyGroups.Should().NotBeNull();
    }
}