using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Repositories;
using WebTimetable.Application.Services;
using Xunit;

namespace WebTimetable.Tests.ServicesTests;

public class NotesServiceTests
{
    private readonly NotesService _notesService;
    
    public NotesServiceTests()
    {
        Expression<Func<NoteEntity, bool>> existing = entity =>
            entity.LessonId == Guid.Empty && entity.Author.Group == string.Empty;
        
        var dbRepositoryMock = new Mock<IDbRepository>();   
        dbRepositoryMock.Setup(x => x.Add<NoteEntity>(null!)).Throws<Exception>();
        dbRepositoryMock.Setup(x => x.Add(It.IsAny<NoteEntity>())).Returns(Task.CompletedTask);
        dbRepositoryMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        dbRepositoryMock.Setup(x => x.Remove(It.IsAny<NoteEntity>()));
        dbRepositoryMock
            .Setup(x => x.Get(It.IsAny<Expression<Func<NoteEntity, bool>>>()))
            .Returns(new List<NoteEntity>().AsQueryable());
        
        _notesService = new NotesService(dbRepositoryMock.Object);
    }
    
    [Fact]
    public async Task NotesService_AddNoteAsync_ReturnTrue()
    {
        // Arrange
        var note = new NoteEntity();
        
        // Act
        var result = await _notesService.AddNoteAsync(note, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void NotesService_GetNoteById_ReturnTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        // Act
        var result = _notesService.GetNoteById(id);
        
        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task NotesService_RemoveNote_NotThrowsAnything()
    {
        // Arrange
        var note = new NoteEntity();
        
        // Act
        var act = async () => await _notesService.RemoveNote(note, CancellationToken.None);
        
        // Assert
        await act.Should().NotThrowAsync();
    }
}