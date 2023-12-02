using System.Linq.Expressions;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Repositories;
using WebTimetable.Application.Services;

namespace WebTimetable.Tests.ApplicationUnitTests.Services;

public class NotesServiceTests
{
    private readonly NotesService _notesService;
    
    public NotesServiceTests()
    {
        Expression<Func<NoteEntity, bool>> existing = entity =>
            entity.LessonId == Guid.Empty && entity.Author.Group == string.Empty;
        
        var mockNotesRepo = new Mock<IRepository<NoteEntity>>();   
        mockNotesRepo.Setup(x => x.AddAsync(null!, CancellationToken.None)).Throws<Exception>();
        mockNotesRepo.Setup(x => x.AddAsync(It.IsAny<NoteEntity>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        mockNotesRepo.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        mockNotesRepo.Setup(x => x.Remove(It.IsAny<NoteEntity>()));
        mockNotesRepo
            .Setup(x => x.Where(It.IsAny<Expression<Func<NoteEntity, bool>>>()))
            .Returns(new List<NoteEntity>().AsQueryable());
        
        _notesService = new NotesService(mockNotesRepo.Object);
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
    public async Task NotesService_AddNoteAsync_ReturnFalse()
    {
        // Arrange
        var note = new NoteEntity
        {
            NoteId = Guid.NewGuid()
        };
        var mockNotesRepo = new Mock<IRepository<NoteEntity>>();   
        mockNotesRepo
            .Setup(x => x.Where(It.IsAny<Expression<Func<NoteEntity, bool>>>()))
            .Returns(new List<NoteEntity> { note }.AsQueryable());
        
        var notesService = new NotesService(mockNotesRepo.Object);
        
        // Act
        var result = await notesService.AddNoteAsync(note, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
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