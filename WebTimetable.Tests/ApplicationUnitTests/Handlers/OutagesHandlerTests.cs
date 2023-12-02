using WebTimetable.Application.Entities;
using WebTimetable.Application.Handlers;
using WebTimetable.Application.Models;
using WebTimetable.Application.Repositories;

namespace WebTimetable.Tests.ApplicationUnitTests.Handlers;

public class DtekOutagesHandlerTests
{
    private readonly OutagesHandler _outagesHandler;
    
    public DtekOutagesHandlerTests()
    {
        var outagesRepoMock = new Mock<IRepository<OutageEntity>>();
        outagesRepoMock.Setup(x => x.FindAsync(It.IsAny<CancellationToken>() ,It.IsAny<object?[]?>()))
            .ReturnsAsync(() => new OutageEntity
            {
                Outages = new List<Outage>
                {
                    new() { Start = new TimeOnly(11, 00), End = new TimeOnly(12, 00) },
                    new() { Start = new TimeOnly(12, 00), End = new TimeOnly(13, 00) },
                    new() { Start = new TimeOnly(13, 00), End = new TimeOnly(14, 00) },
                    new() { Start = new TimeOnly(15, 00), End = new TimeOnly(16, 00) }
                }, 
            });
        
        _outagesHandler = new OutagesHandler(outagesRepoMock.Object);
    }
    
    [Fact]
    public async Task DtekOutagesHandler_ConfigureOutages_ReturnsLessonWithOutages()
    {
        // Arrange
        var schedule = new List<Lesson>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Discipline = "Blah-blah",
                Date = new DateOnly(2011, 11, 11),
                Start = new TimeOnly(11, 30),
                End = new TimeOnly(12, 50)
            }
        };
        
        // Act
        await _outagesHandler.ConfigureOutagesAsync(schedule, "Group 1", "Kyiv", CancellationToken.None);
        
        // Assert
        schedule.Should().HaveCount(1);
        var lesson = schedule.First();
        lesson.Outages.Should().HaveCount(2);
        lesson.Outages.Should().AllSatisfy(x => IsIntervalsIntersects(lesson.Start, lesson.End, x.Start, x.End).Should().BeTrue());
    }
    
    [Fact]
    public async Task DtekOutagesHandler_ConfigureOutages_ReturnsLessonWithoutOutages()
    {
        // Arrange
        var schedule = new List<Lesson>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Discipline = "Blah-blah",
                Date = new DateOnly(2011, 11, 11),
                Start = new TimeOnly(16, 01),
                End = new TimeOnly(17, 00)
            }
        };
        
        // Act
        await _outagesHandler.ConfigureOutagesAsync(schedule, "Group 1", "Kyiv", CancellationToken.None);
        
        // Assert
        schedule.Should().HaveCount(1);
        var lesson = schedule.First();
        lesson.Outages.Should().BeEmpty();
    }
    
    private bool IsIntervalsIntersects(TimeOnly start1, TimeOnly end1, TimeOnly start2, TimeOnly end2)
    {
        var isStartIntersects = start2 <= start1 && start1 < end2;
        var isEndIntersects = start2 < end1 && end1 <= end2;
        var isInside = start1 <= start2 && end1 >= end2;
        return isStartIntersects || isEndIntersects || isInside;
    }
}