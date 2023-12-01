using WebTimetable.Contracts.Requests;
using WebTimetable.Api.Validators;

namespace WebTimetable.Tests.ValidatorsTests;

public class PersonalScheduleValidatorTests
{
    [Fact]
    public void PersonalScheduleRequest_ShouldBeValid()
    {
        var validator = new PersonalScheduleValidator();
        var request = new PersonalScheduleRequest
        {
            StudyGroup = "Test group",
            OutageGroup = "Test outage group",
            Date = DateTime.Now.ToString("yyyy-MM-dd")
        };
        var result = validator.Validate(request);   
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void PersonalScheduleRequest_ShouldBeNotValid()
    {
        var validator = new PersonalScheduleValidator();
        var firstRequest = new PersonalScheduleRequest
        {
            StudyGroup = string.Empty,
            OutageGroup = "Test outage group or test outage group or test outage group or test outage group or test outage group or test outage group or test outage group",
            Date = ""
        };
        var secondRequest = new PersonalScheduleRequest
        {
            StudyGroup = "Test study groups or test study groups or test study groups or test study groups or test study groups or test study groups or test study groups",
            OutageGroup = "",
            Date = "11-SEPTEMBER-2001"
        };
        var firstResult = validator.Validate(firstRequest);   
        var secondResult = validator.Validate(secondRequest);   
        firstResult.IsValid.Should().BeFalse();
        secondResult.IsValid.Should().BeFalse();
    }
}