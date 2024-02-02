using System.Linq.Expressions;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Repositories;
using WebTimetable.Application.Repositories.Abstractions;
using WebTimetable.Application.Services;

namespace WebTimetable.Tests.ApplicationUnitTests.Services;

public class NotesServiceTests
{
    [Fact]
    public async Task NotesService_AddNoteAsync_ReturnTrue()
    {
        // Arrange
        var mockNotesRepo = new Mock<INotesRepository>();
        mockNotesRepo.Setup(x => x.IsNoteExists(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(false);
        mockNotesRepo.Setup(x => x.AddAsync(It.IsAny<NoteEntity>(), It.IsAny<CancellationToken>())).Verifiable();
        var user = new NoteEntity { LessonId = Guid.NewGuid(), Author = new UserEntity { Id = Guid.NewGuid() } };
        var notesService = new NotesService(mockNotesRepo.Object);
        
        // Act
        var result = await notesService.AddNoteAsync(user, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        mockNotesRepo.Verify(x => x.AddAsync(It.IsAny<NoteEntity>(), It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task NotesService_AddNoteAsync_ReturnFalse()
    {
        // Arrange
        var mockNotesRepo = new Mock<INotesRepository>();
        mockNotesRepo.Setup(x => x.IsNoteExists(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(true);
        mockNotesRepo.Setup(x => x.AddAsync(It.IsAny<NoteEntity>(), It.IsAny<CancellationToken>())).Verifiable();
        var user = new NoteEntity { LessonId = Guid.NewGuid(), Author = new UserEntity { Id = Guid.NewGuid() } };
        var notesService = new NotesService(mockNotesRepo.Object);
        
        // Act
        var result = await notesService.AddNoteAsync(user, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
        mockNotesRepo.Verify(x => x.AddAsync(It.IsAny<NoteEntity>(), It.IsAny<CancellationToken>()), Times.Never);

    }

    [Fact]
    public void NotesService_GetNoteById_ReturnNote()
    {
        // Arrange
        var expectedNote = new NoteEntity { NoteId = Guid.NewGuid() };
        var mockNotesRepo = new Mock<INotesRepository>();
        mockNotesRepo.Setup(x => x.GetNoteById(It.Is<Guid>(y => y == expectedNote.NoteId))).Returns(expectedNote);
        var notesService = new NotesService(mockNotesRepo.Object);
        
        // Act
        var result = notesService.GetNoteById(expectedNote.NoteId);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedNote);
    }
    
    [Fact]
    public async Task NotesService_RemoveNote_NotThrowsAnything()
    {
        // Arrange
        var mockNotesRepo = new Mock<INotesRepository>();
        mockNotesRepo.Setup(x => x.RemoveAsync(It.IsAny<NoteEntity>(), It.IsAny<CancellationToken>())).Verifiable();
        var notesService = new NotesService(mockNotesRepo.Object);
        
        // Act
        await notesService.RemoveNote(new NoteEntity(), CancellationToken.None);
        
        // Assert
        mockNotesRepo.Verify(x => x.RemoveAsync(It.IsAny<NoteEntity>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}