using WebTimetable.Application.Entities;
using WebTimetable.Application.Handlers;
using WebTimetable.Application.Models;
using WebTimetable.Application.Repositories.Abstractions;

namespace WebTimetable.Tests.ApplicationUnitTests.Handlers;

public class NotesHandlerTests
{
    [Fact]
    public void NotesHandler_ConfigureNotes_ReturnLessonWithNotes()
    {
        var lessonId = Guid.NewGuid();
        // Arrange
        var schedule = new List<StudentLesson>
        {
            new()
            {
                Id = lessonId,
                Discipline = "Blah-blah"
            }
        };
        var mockRepository = new Mock<INotesRepository>();
        mockRepository.Setup(x => x.GetNotesByLessonId(It.IsAny<Guid>(), It.IsAny<string>()))
            .Returns(new List<NoteEntity>
            {
                new()
                {
                    NoteId = Guid.NewGuid(),
                    Message = "Blah-blah"
                }
            });
        var notesHandler = new NotesHandler(mockRepository.Object);
        
        // Act
        notesHandler.ConfigureNotes(schedule, "Group 1");
        
        // Assert
        schedule.Should().HaveCount(1);
        var itemNotes = schedule.First().Notes;
        itemNotes.Should().HaveCount(1);
    }    

}