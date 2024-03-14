using WebTimetable.Api.Components;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Models;

namespace WebTimetable.Tests.ApiUnitTests.Components;

public class MappingExtensionsTests
{
    [Fact]
    public void MappingExtensions_MapToLessonItems_ReturnsLessonItems()
    {
        // Arrange
        var lessons = new List<StudentLesson>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Discipline = "Test discipline",
                StudyType = "Test study type",
                Cabinet = "Test cabinet",
                Teacher = "Test teacher",
                Outages = new List<Outage>
                {
                    new() { IsDefinite = true },
                    new() { IsDefinite = true },
                    new() { IsDefinite = true }
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                Discipline = "Second test discipline",
                StudyType = "Second test study type",
                Cabinet = "Second test cabinet",
                Teacher = "Second test teacher",
                Outages = new List<Outage>
                {
                    new() { IsDefinite = false },
                    new() { IsDefinite = true },
                    new() { IsDefinite = false }
                }
            }
        };

        // Act
        var lessonItems = lessons.ToLessonDTO();

        // Assert
        lessonItems.Should().NotBeNull();
        lessonItems.Should().HaveCount(2);
        lessonItems.Should().AllSatisfy(x => x.Id.Should().NotBeEmpty());
        lessonItems.Should().AllSatisfy(x => x.Discipline.Should().NotBeNullOrEmpty());
        lessonItems.Should().AllSatisfy(x => x.StudyType.Should().NotBeNullOrEmpty());
        lessonItems.Should().AllSatisfy(x => x.Cabinet.Should().NotBeNullOrEmpty());
        lessonItems.Should().AllSatisfy(x => x.Teacher.Should().NotBeNullOrEmpty());
    }
}