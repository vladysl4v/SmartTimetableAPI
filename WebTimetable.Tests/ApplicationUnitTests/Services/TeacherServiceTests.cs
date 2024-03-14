using WebTimetable.Application.Entities;
using WebTimetable.Application.Handlers.Abstractions;
using WebTimetable.Application.Models;
using WebTimetable.Application.Repositories.Abstractions;
using WebTimetable.Application.Services;

namespace WebTimetable.Tests.ApplicationUnitTests.Services;

public class TeacherServiceTests
{
    [Fact]
    public async Task TeacherService_GetScheduleAsync_ReturnScheduleWithOutages()
    {
        // Arrange
        var mockScheduleHandler = new Mock<IRequestHandler>();
        var mockEventsHandler = new Mock<IEventsHandler>();
        var mockNotesRepository = new Mock<INotesRepository>();
        var mockOutagesHandler = new Mock<IOutagesHandler>();

        mockScheduleHandler.Setup(x => x.GetTeacherSchedule(It.IsAny<DateTime>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new List<TeacherLesson> { new(), new(), new() });
        
        mockOutagesHandler.Setup(x => x.ConfigureOutagesAsync(It.IsAny<IEnumerable<TeacherLesson>>(),
            It.IsAny<string>(), It.IsAny<CancellationToken>())).Verifiable();
        
        var scheduleService = new TeacherService(mockEventsHandler.Object, mockScheduleHandler.Object,
            mockOutagesHandler.Object, mockNotesRepository.Object);
        
        // Act
        var lessons =
            await scheduleService.GetScheduleAsync(DateTime.Now, "Test", "Group 1", CancellationToken.None);
        
        // Assert
        mockOutagesHandler.Verify(
            mr => mr.ConfigureOutagesAsync(It.IsAny<IEnumerable<TeacherLesson>>(), It.IsAny<string>(),
                It.IsAny<CancellationToken>()), Times.Once());
        lessons.Should().NotBeNull();
        lessons.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task TeacherService_GetScheduleAsync_ReturnScheduleWithoutOutages()
    {
        // Arrange
        var mockScheduleHandler = new Mock<IRequestHandler>();
        var mockEventsHandler = new Mock<IEventsHandler>();
        var mockNotesRepository = new Mock<INotesRepository>();
        var mockOutagesHandler = new Mock<IOutagesHandler>();

        mockScheduleHandler.Setup(x => x.GetTeacherSchedule(It.IsAny<DateTime>(), It.IsAny<string>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(new List<TeacherLesson> { new(), new(), new() });
        
        mockOutagesHandler.Setup(x => x.ConfigureOutagesAsync(It.IsAny<IEnumerable<TeacherLesson>>(),
            It.IsAny<string>(), It.IsAny<CancellationToken>())).Verifiable();
        
        var scheduleService = new TeacherService(mockEventsHandler.Object, mockScheduleHandler.Object,
            mockOutagesHandler.Object, mockNotesRepository.Object);
        
        // Act
        var lessons = await scheduleService.GetScheduleAsync(DateTime.Now, "Test", "", CancellationToken.None);
        
        // Assert
        mockOutagesHandler.Verify(
            mr => mr.ConfigureOutagesAsync(It.IsAny<IEnumerable<TeacherLesson>>(), It.IsAny<string>(),
                It.IsAny<CancellationToken>()), Times.Never());
        
        lessons.Should().NotBeNull();
        lessons.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task TeacherService_GetLessonDetails_ReturnLessonDetails()
    {
        // Arrange
        var mockNotesHandler = new Mock<INotesRepository>();
        var mockScheduleHandler = new Mock<IRequestHandler>();
        var mockOutagesHandler = new Mock<IOutagesHandler>();
        var mockEventsHandler = new Mock<IEventsHandler>();
        
        mockEventsHandler
            .Setup(x => x.GetEventsAsync(It.IsAny<DateOnly>(), It.IsAny<TimeOnly>(), It.IsAny<TimeOnly>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(new List<Event>()).Verifiable();
        
        mockNotesHandler.Setup(x => x.GetNotesByLessonId(It.IsAny<Guid>(), It.IsAny<string>()))
            .Returns(new List<NoteEntity>()).Verifiable();
        
        var scheduleService = new TeacherService(mockEventsHandler.Object, mockScheduleHandler.Object,
            mockOutagesHandler.Object, mockNotesHandler.Object);
        
        var lesson = new TeacherLesson
        {
            Id = Guid.NewGuid(),
            Discipline = "Chin-choppa",
            Date = new DateOnly(2011, 11, 11),
            Start = new TimeOnly(14, 30),
            End = new TimeOnly(15, 50)
        };
        
        // Act
        var details = await scheduleService.GetLessonDetails(lesson.Id, lesson.Date, lesson.Start, lesson.End,
            string.Empty, CancellationToken.None);

        // Assert
        mockEventsHandler.Verify(
            x => x.GetEventsAsync(It.IsAny<DateOnly>(), It.IsAny<TimeOnly>(), It.IsAny<TimeOnly>(),
                It.IsAny<CancellationToken>()), Times.Once());
        mockNotesHandler.Verify(x => x.GetNotesByLessonId(It.IsAny<Guid>(), It.IsAny<string>()), Times.Once());
        
        details.Events.Should().BeEmpty();
        details.Notes.Should().BeEmpty();
        details.Id.Should().Be(lesson.Id);
    }
    
    
    [Fact]
    public async Task TeacherService_GetFacultiesAsync_ReturnFaculties()
    {
        // Arrange
        var mockScheduleHandler = new Mock<IRequestHandler>();
        var mockNotesHandler = new Mock<INotesRepository>();
        var mockOutagesHandler = new Mock<IOutagesHandler>();
        var mockEventsHandler = new Mock<IEventsHandler>();
        
        mockScheduleHandler.Setup(x => x.GetTeacherFaculties(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<KeyValuePair<string, string>> { new(), new(), new() });
        
        var scheduleService = new TeacherService(mockEventsHandler.Object, mockScheduleHandler.Object,
            mockOutagesHandler.Object, mockNotesHandler.Object);
        
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
        var mockNotesHandler = new Mock<INotesRepository>();
        var mockScheduleHandler = new Mock<IRequestHandler>();
        var mockOutagesHandler = new Mock<IOutagesHandler>();
        var mockEventsHandler = new Mock<IEventsHandler>();
        
        mockScheduleHandler.Setup(x => x.GetTeacherChairs(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<KeyValuePair<string, string>> { new(), new(), new() });
        
        var scheduleService = new TeacherService(mockEventsHandler.Object, mockScheduleHandler.Object,
            mockOutagesHandler.Object, mockNotesHandler.Object);
        
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
        var mockNotesHandler = new Mock<INotesRepository>();
        var mockOutagesHandler = new Mock<IOutagesHandler>();
        var mockEventsHandler = new Mock<IEventsHandler>();
        
        mockScheduleHandler.Setup(x => x.GetTeacherEmployees(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<KeyValuePair<string, string>> { new(), new(), new() });
        
        var scheduleService = new TeacherService(mockEventsHandler.Object, mockScheduleHandler.Object,
            mockOutagesHandler.Object, mockNotesHandler.Object);
        
        // Act
        var employees = await scheduleService.GetEmployeesAsync("NNN", "FYP", CancellationToken.None);
        
        // Assert
        employees.Should().NotBeNull();
        employees.Should().HaveCount(3);
    }
}