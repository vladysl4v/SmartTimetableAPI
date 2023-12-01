using WebTimetable.Api.Validators;
using WebTimetable.Contracts.Requests;

namespace WebTimetable.Tests.ValidatorsTests;

public class AddNoteValidatorTests
{
    [Fact]
    public void AddNoteRequest_ShouldBeValid()
    {
        var validator = new AddNoteValidator();
        var request = new AddNoteRequest
        {
            LessonId = Guid.NewGuid(),
            Message = "Test message"
        };
        var result = validator.Validate(request);
        result.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public void AddNoteValidator_MessageShouldBeNotValid()
    {
        var validator = new AddNoteValidator();
        var firstRequest = new AddNoteRequest
        {
            LessonId = Guid.NewGuid(),
            Message = "Very long message but very long message but very long message but very long message but very long message but very long message but very long message but very long message but very long message but very long message but very long message but very long message but very long message"
        };
        var secondRequest = new AddNoteRequest
        {
            LessonId = Guid.Empty,
            Message = "Test message"
        };
        var firstResult = validator.Validate(firstRequest);
        var secondResult = validator.Validate(secondRequest);
        firstResult.IsValid.Should().BeFalse();
        secondResult.IsValid.Should().BeFalse();
    }
}