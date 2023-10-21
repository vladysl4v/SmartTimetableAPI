using WebTimetable.Application.Entities;

namespace WebTimetable.Application.Services.Abstractions;

public interface INotesService
{
    public Task<bool> AddNoteAsync(NoteEntity note, CancellationToken token);
    public Task RemoveNote(NoteEntity note, CancellationToken token);
    public NoteEntity? GetNoteById(Guid id);
}