using WebTimetable.Api.Validators;
using WebTimetable.Contracts.Requests;

namespace WebTimetable.Tests.ApiUnitTests.Validators;

public class TeacherScheduleValidatorTests
{
    [Fact]
    public void TeacherScheduleRequest_ShouldBeValid()
    {
        // Arrange
        var validator = new TeacherScheduleValidator();
        var request = new TeacherScheduleRequest
        {
            TeacherId = "Test teacher",
            OutageGroup = "Test outage group",
            Date = DateTime.Now.ToString("yyyy-MM-dd")
        };
        
        // Act
        var result = validator.Validate(request);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [InlineData("Test teacherIds or test teacherIds or test teacherIds or test teacherIds or test teacherIds or test teacherIds or test teacherIds", "Test outage group", "2001-09-11")]
    [InlineData("Test teacherId", "Test outage group or test outage group or test outage group or test outage group or test outage group or test outage group or test outage group", "2001-09-11")]
    [InlineData("Test teacherId", "Test outage group", "")]
    [InlineData("Test teacherId", "Test outage group", "11-CUCUMBER-2001")]
    public void TeacherScheduleRequest_ShouldBeNotValid(string teacherId, string outageGroup, string date)
    {
        // Arrange
        var validator = new TeacherScheduleValidator();
        var request = new TeacherScheduleRequest
        {
            TeacherId = teacherId,
            OutageGroup = outageGroup,
            Date = date
        };
        
        // Act
        var result = validator.Validate(request);   
        
        // Assert
        result.IsValid.Should().BeFalse();
    }
}