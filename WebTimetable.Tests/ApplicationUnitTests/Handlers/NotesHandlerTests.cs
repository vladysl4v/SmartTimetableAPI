using System.Linq.Expressions;
using Neleus.LambdaCompare;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Handlers;
using WebTimetable.Application.Models;
using WebTimetable.Application.Repositories;

namespace WebTimetable.Tests.ApplicationUnitTests.Handlers;

public class NotesHandlerTests
{
    private readonly NotesHandler _notesHandler;
    private readonly NoteEntity _existingNote;
    
    public NotesHandlerTests()
    {
        _existingNote = new NoteEntity
        {
            NoteId = Guid.NewGuid(),
            LessonId = Guid.NewGuid(),
            Message = "Test message",
            AuthorId = Guid.NewGuid(),
            Author = new UserEntity
            {
                FullName = "Blah Blah Blah",
                Group = "YEP"
            }
        };
        
        var notes = new[] { _existingNote }.AsQueryable();
        var dbRepositoryMock = new Mock<IDbRepository>();
        Expression<Func<NoteEntity, bool>> existing = entity =>
            entity.LessonId == _existingNote.LessonId && entity.Author.Group == _existingNote.Author.Group;
        
        dbRepositoryMock.Setup(action => action.Get<NoteEntity>(It.Is<Expression<Func<NoteEntity, bool>>>(
            expr => Lambda.ExpressionsEqual(expr, existing)))).Returns(notes);
        
        _notesHandler = new NotesHandler(dbRepositoryMock.Object);
    }
    
    [Fact]
    public void NotesHandler_ConfigureNotes_ReturnLessonWithNotes()
    {
        // Arrange
        var schedule = new List<Lesson>
        {
            new()
            {
                Id = _existingNote.LessonId,
                Discipline = "Blah-blah"
            }
        };
        
        // Act
        _notesHandler.ConfigureNotes(schedule, _existingNote.Author.Group, _existingNote.AuthorId);
        
        // Assert
        schedule.Should().HaveCount(1);
        var itemNotes = schedule.First().Notes;
        itemNotes.Should().HaveCount(1);
        
        itemNotes.First().NoteId.Should().Be(_existingNote.NoteId);
        itemNotes.First().Message.Should().Be(_existingNote.Message);
    }    
    [Fact]
    public void NotesHandler_ConfigureNotes_ReturnLessonWithoutNotes()
    {
        // Arrange
        var schedule = new List<Lesson>
        {
            new()
            {
                Id = _existingNote.LessonId,
                Discipline = "Blah-blah"
            }
        };
        
        // Act
        _notesHandler.ConfigureNotes(schedule, "POG", _existingNote.AuthorId);
        
        // Assert
        schedule.Should().HaveCount(1);
        var itemNotes = schedule.First().Notes;
        itemNotes.Should().BeEmpty();
    }
}