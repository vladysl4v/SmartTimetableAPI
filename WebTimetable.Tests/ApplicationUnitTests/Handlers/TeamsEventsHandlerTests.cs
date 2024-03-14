using Microsoft.Graph.Models;
using WebTimetable.Application.Handlers;
using WebTimetable.Application.Models;
using WebTimetable.Tests.TestingUtilities;
using GraphEvent = Microsoft.Graph.Models.Event;

namespace WebTimetable.Tests.ApplicationUnitTests.Handlers;

public class TeamsEventsHandlerTests
{
    private readonly int _utcOffset = TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time").
        GetUtcOffset(DateTime.UtcNow).Hours;

    [Fact]
    public async Task TeamsEventsHandler_GetEventsAsync_ReturnEvents()
    {
        // Arrange
        var mockGraphFactory = new MockGraphClientFactory().Setup(new EventCollectionResponse
        {
            Value = new List<GraphEvent>
            {
                new()
                {
                    IsCancelled = false,
                    OnlineMeeting = new OnlineMeetingInfo { JoinUrl = "https://first-subject.io" },
                    Start = new DateTimeTimeZone { DateTime = $"2011-11-11T{19-_utcOffset}:48:00.0000000", TimeZone = "FLE Standard Time" },
                    End = new DateTimeTimeZone { DateTime = $"2011-11-11T{17-_utcOffset}:30:00.0000000", TimeZone = "FLE Standard Time" },
                    Subject = "SecondSubject",
                },
                new()
                {
                    IsCancelled = false,
                    OnlineMeeting = new OnlineMeetingInfo { JoinUrl = null },
                    Start = new DateTimeTimeZone { DateTime = $"2011-11-11T0{18-_utcOffset}:30:00.0000000", TimeZone = "FLE Standard Time"},
                    End = new DateTimeTimeZone { DateTime = $"2011-11-11T0{19-_utcOffset}:55:00.0000000", TimeZone = "FLE Standard Time" },
                    Subject = "FirstSubject",
                },
                new()
                {
                    IsCancelled = false,
                    OnlineMeeting = new OnlineMeetingInfo { JoinUrl = "https://second-subject.io" },
                    Start = new DateTimeTimeZone { DateTime = $"2011-11-11T{15-_utcOffset}:48:00.0000000", TimeZone = "FLE Standard Time" },
                    End = new DateTimeTimeZone { DateTime = $"2011-11-11T{16-_utcOffset}:30:00.0000000", TimeZone = "FLE Standard Time" },
                    Subject = "SecondSubject",
                }
            }
        });
        var teamsHandler = new TeamsEventsHandler(mockGraphFactory.CreateClient());

        var lesson = new StudentLesson
        {
            Id = Guid.NewGuid(),
            Discipline = "Chin-choppa",
            Date = new DateOnly(2011, 11, 11),
            Start = new TimeOnly(14, 30),
            End = new TimeOnly(15, 50)
        };

        // Act
        var events = await teamsHandler.GetEventsAsync(lesson.Date, lesson.Start, lesson.End, CancellationToken.None);

        // Assert
        events.Should().HaveCount(2);
        events.Should().AllSatisfy(x => x.Link.Should().NotBeNull());
        events.Should().AllSatisfy(x => x.Title.Should().NotBeNull());
    }

    [Fact]
    public async Task TeamsEventHandler_ConfigureEvents_ReturnsListWithoutEvents()
    {
        // Arrange
        var mockGraphFactory = new MockGraphClientFactory().Setup((EventCollectionResponse)null!);
        var mockTeamsHandler = new TeamsEventsHandler(mockGraphFactory.CreateClient());
        
        var lesson = new StudentLesson
        {
            Id = Guid.NewGuid(),
            Discipline = "Chin-choppa",
            Date = new DateOnly(2011, 11, 11),
            Start = new TimeOnly(14, 30),
            End = new TimeOnly(15, 50)
        };

        // Act
        var events = await mockTeamsHandler.GetEventsAsync(lesson.Date, lesson.Start, lesson.End, CancellationToken.None);
        
        // Assert
        events.Should().NotBeNull();
        events.Should().BeEmpty();
    }
}