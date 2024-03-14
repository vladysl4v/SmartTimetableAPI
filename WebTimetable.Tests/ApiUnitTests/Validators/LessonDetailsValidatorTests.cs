using WebTimetable.Api.Validators;
using WebTimetable.Contracts.Requests;

namespace WebTimetable.Tests.ApiUnitTests.Validators;

public class LessonDetailsValidatorTests
{
    [Theory]
    [InlineData("2021-09-11", "08:00", "10:00")]
    [InlineData("2021/09/11", "13:00:00", "15:00:00")]
    public void LessonDetailsRequest_ShouldBeValid(string date, string start, string end)
    {
        // Arrange
        var validator = new LessonDetailsValidator();
        var request = new LessonDetailsRequest
        {
            StartTime = start,
            EndTime = end,
            Date = date
        };
        
        // Act
        var result = validator.Validate(request);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [InlineData("11-CUCUMBER-2001", "08:00", "10:00")]
    [InlineData("2021-09-11", "WHAT", "15:00:00")]
    [InlineData("2021-09-11", "08:00", "Dinner")]
    public void LessonDetailsRequest_ShouldBeNotValid(string date, string start, string end)
    {
        // Arrange
        var validator = new LessonDetailsValidator();
        var request = new LessonDetailsRequest
        {
            StartTime = start,
            EndTime = end,
            Date = date
        };

        // Act
        var result = validator.Validate(request);   
        
        // Assert
        result.IsValid.Should().BeFalse();
    }
}