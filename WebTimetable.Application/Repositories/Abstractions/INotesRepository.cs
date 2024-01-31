using WebTimetable.Application.Entities;
using WebTimetable.Application.Models;

namespace WebTimetable.Application.Repositories.Abstractions;

public interface INotesRepository
{
    public List<NoteEntity> GetNotesByLessonId(Guid lessonId, string userGroup);
    public bool IsNoteExists(Guid authorId, Guid lessonId);
    public NoteEntity? GetNoteById(Guid noteId);
    Task AddAsync(NoteEntity note, CancellationToken token);
    Task RemoveAsync(NoteEntity note, CancellationToken token);
}