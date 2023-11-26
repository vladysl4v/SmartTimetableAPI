using FluentAssertions.Extensions;
using WebTimetable.Application.Exceptions;
using WebTimetable.Application.Handlers;
using WebTimetable.Tests.TestingUtilities;

namespace WebTimetable.Tests.HandlersTests;

public class VnzOsvitaScheduleHandlerTests
{
    private readonly VnzOsvitaScheduleHandler _vnzOsvitaSchedule;

    public VnzOsvitaScheduleHandlerTests()
    {
        var mockResponse = "{\"d\":[{\"__type\":\"VnzWeb.BetaSchedule+ScheduleDataRow\",\"study_time\":\"14:50-16:10\",\"study_time_begin\":\"14:50\",\"study_time_end\":\"16:10\",\"week_day\":\"Monday\",\"full_date\":\"11.11.2011\",\"discipline\":\"Physics\",\"study_type\":\"Lecture\",\"cabinet\":\"005\",\"employee\":\"Great Greatness Greatier\",\"study_subgroup\":null},{\"__type\":\"VnzWeb.BetaSchedule+ScheduleDataRow\",\"study_time\":\"16:25-17:45\",\"study_time_begin\":\"16:25\",\"study_time_end\":\"17:45\",\"week_day\":\"Monday\",\"full_date\":\"11.11.2011\",\"discipline\":\"Math\",\"study_type\":\"Practice\",\"cabinet\":\"010\",\"employee\":\"Empty Emptiness Emptier\",\"study_subgroup\":null}]}";
        var mockHttpFactory = new MockHttpFactory()
            .Setup("")
            .Setup(x => x.RequestUri.AbsoluteUri.Contains("01.11.2011"), mockResponse)
            .Setup(x => x.RequestUri.AbsoluteUri.Contains("02.12.2012"), "{\"d\":[]}");
        _vnzOsvitaSchedule = new VnzOsvitaScheduleHandler(mockHttpFactory);
    }
    
    [Fact]
    public async Task VnzOsvitaScheduleHandler_GetSchedule_ReturnSchedule()
    {
        // Arrange
        var expectedLessons = new List<string> { "Physics", "Math" };
        // Act
        var result = await _vnzOsvitaSchedule.GetSchedule(1.November(2011), "NNN", default);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().AllSatisfy(x => expectedLessons.Contains(x.Discipline));
    }
    
    [Fact]
    public async Task VnzOsvitaScheduleHandler_GetSchedule_ReturnEmptyList()
    {
        // Act
        var result = await _vnzOsvitaSchedule.GetSchedule(2.December(2012), "NNN", default);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
    
    [Fact]
    public async Task VnzOsvitaScheduleHandler_GetSchedule_ThrowsException()
    {
        // Act
        var act = async () => await _vnzOsvitaSchedule.GetSchedule(10.October(2010), "NNN", default);
        
        // Assert
        await act.Should().ThrowAsync<InternalServiceException>();
    }
}