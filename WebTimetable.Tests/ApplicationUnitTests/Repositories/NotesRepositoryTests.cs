using WebTimetable.Application.Entities;
using WebTimetable.Application.Repositories;
using WebTimetable.Tests.TestingUtilities;

namespace WebTimetable.Tests.ApplicationUnitTests.Repositories;

public class NotesRepositoryTests
{
    private readonly MockDataContext _mockDataContext = new();

    [Fact]
    public void NotesRepository_GetNotesByLessonId_ReturnsNotes()
    {
        // Arrange
        var necessaryLessonId = Guid.NewGuid();
        var necessaryGroup = "Group 1";
        _mockDataContext.AddRange(new List<NoteEntity>
        {
            new() { Message = "", LessonId = Guid.NewGuid(), Author = new UserEntity { Group = necessaryGroup, FullName = "" } },
            new() { Message = "", LessonId = necessaryLessonId, Author = new UserEntity { Group = necessaryGroup, FullName = "" } },
            new() { Message = "", LessonId = Guid.NewGuid(), Author = new UserEntity { Group = "Group 2", FullName = "" } },
            new() { Message = "", LessonId = Guid.NewGuid(), Author = new UserEntity { Group = "Group 2", FullName = "" } },
        });

        var notesRepository = new NotesRepository(_mockDataContext);

        // Act
        var notes = notesRepository.GetNotesByLessonId(necessaryLessonId, necessaryGroup);
        
        // Assert
        notes.Should().NotBeNull();
        notes.Should().HaveCount(1);
        var note = notes.First();
        note.LessonId.Should().Be(necessaryLessonId);
        note.Author.Group.Should().Be(necessaryGroup);
    }
    
    [Fact]
    public void NotesRepository_GetNotesByLessonId_ReturnsNothing()
    {
        // Arrange

        _mockDataContext.AddRange(new List<NoteEntity>
        {
            new() { Message = "", LessonId = Guid.NewGuid(), Author = new UserEntity { Group = "Random group", FullName = "" } },
            new() { Message = "", LessonId = Guid.NewGuid(), Author = new UserEntity { Group = "Random group", FullName = "" } },
        });
        var notesRepository = new NotesRepository(_mockDataContext);

        // Act
        var notes = notesRepository.GetNotesByLessonId(Guid.NewGuid(), "Group 0");
        
        // Assert
        notes.Should().NotBeNull();
        notes.Should().BeEmpty();
    }
    
    [Fact]
    public void NotesRepository_IsNoteExists_ReturnTrue()
    {
        // Arrange
        var necessaryLessonId = Guid.NewGuid();
        var necessaryAuthorId = Guid.NewGuid();
        _mockDataContext.AddRange(new List<NoteEntity>
        {
            new() { Message = "", LessonId = Guid.NewGuid(), Author = new UserEntity { FullName = "", Group = "Group 2" } },
            new() { Message = "", LessonId = necessaryLessonId, Author = new UserEntity { FullName = "", Id = necessaryAuthorId, Group = "Group 2"} },
            new() { Message = "", LessonId = Guid.NewGuid(), Author = new UserEntity { FullName = "", Group = "Group 2" } },
        });
        var notesRepository = new NotesRepository(_mockDataContext);

        // Act
        var isNoteExists = notesRepository.IsNoteExists(necessaryAuthorId, necessaryLessonId);
        
        // Assert
        isNoteExists.Should().BeTrue();
    }
    
    [Fact]
    public void NotesRepository_GetNoteById_ReturnsNote()
    {
        // Arrange
        var necessaryNoteId = Guid.NewGuid();
        _mockDataContext.AddRange(new List<NoteEntity>
        {
            new() { NoteId = Guid.NewGuid(), Message = "Wrong message" },
            new() { NoteId = necessaryNoteId, Message = "Test message" },
            new() { NoteId = Guid.NewGuid(), Message = "Wrong message" },
        });
        var notesRepository = new NotesRepository(_mockDataContext);
        
        // Act
        var note = notesRepository.GetNoteById(necessaryNoteId);
        
        // Assert
        note.Should().NotBeNull();
        note!.NoteId.Should().Be(necessaryNoteId);
        note.Message.Should().Be("Test message");
    }
    
    [Fact]
    public async Task NotesRepository_AddAsync_SuccessfullyAddsNote()
    {
        // Arrange
        var necessaryNote = new NoteEntity
        {
            NoteId = Guid.NewGuid(),
            Message = "Test message"
        };
        var notesRepository = new NotesRepository(_mockDataContext);
        
        // Act
        await notesRepository.AddAsync(necessaryNote, CancellationToken.None);
        
        // Assert
        _mockDataContext.Notes.Should().HaveCount(1);
        _mockDataContext.Notes.Should().Contain(necessaryNote);
    }
    
    [Fact]
    public async Task NotesRepository_RemoveAsync_SuccessfullyRemovesNote()
    {
        // Arrange
        var necessaryNote = new NoteEntity
        {
            NoteId = Guid.NewGuid(),
            Message = "Test message"
        };
        _mockDataContext.Notes.Add(necessaryNote);
        var notesRepository = new NotesRepository(_mockDataContext);
        
        // Act
        await notesRepository.RemoveAsync(necessaryNote, CancellationToken.None);
        
        // Assert
        _mockDataContext.Notes.Should().BeEmpty();
    }
}