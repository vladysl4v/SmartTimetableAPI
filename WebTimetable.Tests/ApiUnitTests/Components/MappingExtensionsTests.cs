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
                },
                Events = new List<Event>
                {
                    new()
                    {
                        StartTime = new TimeOnly(14, 50),
                        EndTime = new TimeOnly(16, 10)
                    },
                    new()
                    {
                        StartTime = new TimeOnly(16, 25),
                        EndTime = new TimeOnly(17, 45)
                    }
                },
                Notes = new List<NoteEntity>
                {
                    new()
                    {
                        Message = "Test message",
                        NoteId = Guid.NewGuid(),
                        AuthorId = Guid.NewGuid(),
                        Author = new UserEntity { FullName = "Test name" }
                    },
                    new()
                    {
                        Message = "Second test message",
                        NoteId = Guid.NewGuid(),
                        AuthorId = Guid.NewGuid(),
                        Author = new UserEntity { FullName = "Second test name" }
                    }
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
                },
                Events = new List<Event>
                {
                    new()
                    {
                        StartTime = new TimeOnly(14, 50),
                        EndTime = new TimeOnly(16, 10)
                    },
                    new()
                    {
                        StartTime = new TimeOnly(16, 25),
                        EndTime = new TimeOnly(17, 45)
                    }
                },
                Notes = new List<NoteEntity>
                {
                    new()
                    {
                        Message = "Test message",
                        NoteId = Guid.NewGuid(),
                        AuthorId = Guid.NewGuid(),
                        Author = new UserEntity { FullName = "Test name" }
                    },
                    new()
                    {
                        Message = "Second test message",
                        NoteId = Guid.NewGuid(),
                        AuthorId = Guid.NewGuid(),
                        Author = new UserEntity { FullName = "Second test name" }
                    }
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