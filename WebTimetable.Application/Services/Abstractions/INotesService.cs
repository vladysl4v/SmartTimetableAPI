using WebTimetable.Application.Entities;

namespace WebTimetable.Application.Services.Abstractions;

public interface INotesService
{
    public List<NoteEntity> GetNotesByLessonId(Guid lessonId, string group);
    public Task AddNoteAsync(NoteEntity note);
}