using FluentAssertions;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Moq;
using WebTimetable.Application.Handlers.Events;
using WebTimetable.Application.Models;
using Xunit;

using GraphEvent = Microsoft.Graph.Models.Event;

namespace WebTimetable.Tests.HandlersTests.EventsTests;

public class TeamsEventsHandlerTests
{
    private readonly int _utcOffset = TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time").
        GetUtcOffset(DateTime.UtcNow).Hours;

    [Fact]
    public async Task TeamsEventsHandler_ConfigureEvents_ReturnLessonWithEvents()
    {
        // Arrange
        var mockTeamsHandler = CreateMockTeamsEventsHandler(new EventCollectionResponse
        {
            Value = new List<GraphEvent>
            {
                new()
                {
                    IsCancelled = false,
                    OnlineMeeting = new OnlineMeetingInfo { JoinUrl = "https://first-subject.io" },
                    Start = new DateTimeTimeZone { DateTime = $"2011-11-11T0{11-_utcOffset}:30:00.0000000", TimeZone = "FLE Standard Time"},
                    End = new DateTimeTimeZone { DateTime = $"2011-11-11T0{11-_utcOffset}:55:00.0000000", TimeZone = "FLE Standard Time" },
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
        
        var lessons = new List<Lesson>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Discipline = "Blah-blah",
                Date = new DateOnly(2011, 11, 11),
                Start = new TimeOnly(11, 30),
                End = new TimeOnly(12, 50)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Discipline = "Boom boom",
                Date = new DateOnly(2011, 11, 11),
                Start = new TimeOnly(13, 05),
                End = new TimeOnly(14, 25)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Discipline = "Chin-choppa",
                Date = new DateOnly(2011, 11, 11),
                Start = new TimeOnly(14, 30),
                End = new TimeOnly(15, 50)
            }
        };

        // Act
        await mockTeamsHandler.ConfigureEvents(lessons);

        // Assert
        lessons.Should().HaveCount(3);
        lessons.First(x => x.Discipline == "Boom boom").Events.Should().BeEmpty();
        lessons.Where(x => x.Discipline != "Boom boom").Should().AllSatisfy(x => x.Events.Should().NotBeEmpty());
    }

    [Fact]
    public async Task TeamsEventHandler_ConfigureEvents_ReturnsListWithoutEvents()
    {
        // Arrange
        var mockTeamsHandler = CreateMockTeamsEventsHandler(null);
        
        var lessons = new List<Lesson>
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
        await mockTeamsHandler.ConfigureEvents(lessons);
        
        // Assert
        lessons.Should().HaveCount(1);
        lessons.First().Events.Should().BeNull();
    }

    private TeamsEventsHandler CreateMockTeamsEventsHandler(EventCollectionResponse? requiredResponse)
    {
        var mockRequestAdapter = new Mock<IRequestAdapter>();

        mockRequestAdapter
            .Setup(a => a.SendAsync(It.IsAny<RequestInformation>(), It.IsAny<ParsableFactory<EventCollectionResponse>>(), It.IsAny<Dictionary<string, ParsableFactory<IParsable>>?>(),  It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => requiredResponse);
        
        var mockGraphServiceClient = new Mock<GraphServiceClient>(mockRequestAdapter.Object, "");
        return new TeamsEventsHandler(mockGraphServiceClient.Object);
    }
}