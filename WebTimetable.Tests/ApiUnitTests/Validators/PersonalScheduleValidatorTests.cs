using WebTimetable.Api.Validators;
using WebTimetable.Contracts.Requests;

namespace WebTimetable.Tests.ApiUnitTests.Validators;

public class PersonalScheduleValidatorTests
{
    [Fact]
    public void PersonalScheduleRequest_ShouldBeValid()
    {
        // Arrange
        var validator = new PersonalScheduleValidator();
        var request = new PersonalScheduleRequest
        {
            StudyGroup = "Test group",
            OutageGroup = "Test outage group",
            Date = DateTime.Now.ToString("yyyy-MM-dd")
        };
        
        // Act
        var result = validator.Validate(request);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [InlineData("Test study groups or test study groups or test study groups or test study groups or test study groups or test study groups or test study groups", "Test outage group", "2001-09-11")]
    [InlineData("Test study group", "Test outage group or test outage group or test outage group or test outage group or test outage group or test outage group or test outage group", "2001-09-11")]
    [InlineData("Test study group", "Test outage group", "")]
    [InlineData("Test study group", "Test outage group", "11-CUCUMBER-2001")]
    public void PersonalScheduleRequest_ShouldBeNotValid(string studyGroup, string outageGroup, string date)
    {
        // Arrange
        var validator = new PersonalScheduleValidator();
        var request = new PersonalScheduleRequest
        {
            StudyGroup = studyGroup,
            OutageGroup = outageGroup,
            Date = date
        };
        
        // Act
        var result = validator.Validate(request);   
        
        // Assert
        result.IsValid.Should().BeFalse();
    }
}