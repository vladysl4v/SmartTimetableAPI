using WebTimetable.Api.Validators;
using WebTimetable.Contracts.Requests;

namespace WebTimetable.Tests.ApiUnitTests.Validators;

public class AddNoteValidatorTests
{
    [Fact]
    public void AddNoteRequest_ShouldBeValid()
    {
        // Arrange
        var validator = new AddNoteValidator();
        var request = new AddNoteRequest
        {
            LessonId = Guid.NewGuid(),
            Message = "Test message"
        };
        
        // Act
        var result = validator.Validate(request);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }
    
    [Theory]
    [InlineData("BE6C16BF-7022-4521-A777-C60884E3478A", "Very long message but very long message but very long message but very long message but very long message but very long message but very long message but very long message but very long message but very long message but very long message but very long message but very long message")]
    [InlineData("00000000-0000-0000-0000-000000000000", "Test message")]
    public void AddNoteValidator_MessageShouldBeNotValid(string lessonId, string message)
    {
        // Arrange
        var validator = new AddNoteValidator();
        var request = new AddNoteRequest
        {
            LessonId = Guid.Parse(lessonId),
            Message = message};
        
        // Act
        var result = validator.Validate(request);
        
        // Assert
        result.IsValid.Should().BeFalse();
    }
}