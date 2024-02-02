using WebTimetable.Application.Entities;
using WebTimetable.Application.Handlers;
using WebTimetable.Application.Models;
using WebTimetable.Application.Repositories;
using WebTimetable.Tests.TestingUtilities;

namespace WebTimetable.Tests.ApplicationUnitTests.Handlers;

public class OutagesHandlerTests
{
    private readonly OutagesHandler _outagesHandler;
    
    public OutagesHandlerTests()
    {
        var mockDataContext = new MockDataContext();
        mockDataContext.AddRange(new OutageEntity
            {
                Group = "1st",
                City = "Kyiv",
                DayOfWeek = DayOfWeek.Monday,
                Outages = new List<Outage>
                {
                    new() { Start = new TimeOnly(11, 00), End = new TimeOnly(12, 00) },
                }
            },
            new OutageEntity
            {
                Group = "2nd",
                City = "Kyiv",
                DayOfWeek = DayOfWeek.Monday,
                Outages = new List<Outage>
                {
                    new() { Start = new TimeOnly(13, 00), End = new TimeOnly(14, 00) },
                    new() { Start = new TimeOnly(15, 00), End = new TimeOnly(16, 00) }
                },
            });
        
        var outagesRepository = new OutagesRepository(mockDataContext);
        _outagesHandler = new OutagesHandler(outagesRepository);
    }
    
    [Fact]
    public async Task OutagesHandler_ConfigureOutages_ReturnsLessonWithOutages()
    {
        // Arrange
        var schedule = new List<StudentLesson>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Discipline = "Blah-blah",
                Date = new DateOnly(2024, 02, 05),
                Start = new TimeOnly(11, 30),
                End = new TimeOnly(12, 50)
            }
        };
        
        // Act
        await _outagesHandler.ConfigureOutagesAsync(schedule, "1st", CancellationToken.None);
        
        // Assert
        schedule.Should().HaveCount(1);
        var lesson = schedule.First();
        lesson.Outages.Should().HaveCount(1);
        lesson.Outages.Should().AllSatisfy(x => IsIntervalsIntersects(lesson.Start, lesson.End, x.Start, x.End).Should().BeTrue());
    }
    
    [Fact]
    public async Task OutagesHandler_ConfigureOutages_ReturnsLessonWithoutOutages()
    {
        // Arrange
        var schedule = new List<StudentLesson>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Discipline = "Blah-blah",
                Date = new DateOnly(2024, 02, 07),
                Start = new TimeOnly(11, 30),
                End = new TimeOnly(12, 50)
            }
        };
        
        // Act
        await _outagesHandler.ConfigureOutagesAsync(schedule, "1st", CancellationToken.None);
        
        // Assert
        schedule.Should().HaveCount(1);
        var lesson = schedule.First();
        lesson.Outages.Should().BeEmpty();
        lesson.Outages.Should();
    }
    
    private bool IsIntervalsIntersects(TimeOnly start1, TimeOnly end1, TimeOnly start2, TimeOnly end2)
    {
        var isStartIntersects = start2 <= start1 && start1 < end2;
        var isEndIntersects = start2 < end1 && end1 <= end2;
        var isInside = start1 <= start2 && end1 >= end2;
        return isStartIntersects || isEndIntersects || isInside;
    }
}