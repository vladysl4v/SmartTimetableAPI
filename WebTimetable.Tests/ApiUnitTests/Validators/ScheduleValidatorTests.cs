using WebTimetable.Api.Validators;
using WebTimetable.Contracts.Requests;

namespace WebTimetable.Tests.ApiUnitTests.Validators;

public class ScheduleValidatorTests
{
    [Fact]
    public void StudentScheduleRequest_ShouldBeValid()
    {
        // Arrange
        var validator = new ScheduleRequestValidator();
        var request = new ScheduleRequest
        {
            Identifier = "Test group",
            Date = DateTime.Now.ToString("yyyy-MM-dd")
        };
        
        // Act
        var result = validator.Validate(request);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [InlineData("Test study groups or test study groups or test study groups or test study groups or test study groups or test study groups or test study groups", "2001-09-11")]
    [InlineData("Test study group", "")]
    [InlineData("Test study group", "11-CUCUMBER-2001")]
    public void StudentScheduleRequest_ShouldBeNotValid(string studyGroup, string date)
    {
        // Arrange
        var validator = new ScheduleRequestValidator();
        var request = new ScheduleRequest
        {
            Identifier = studyGroup,
            Date = date
        };
        
        // Act
        var result = validator.Validate(request);   
        
        // Assert
        result.IsValid.Should().BeFalse();
    }
}