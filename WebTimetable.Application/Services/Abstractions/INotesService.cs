using WebTimetable.Application.Entities;
using WebTimetable.Application.Models;

namespace WebTimetable.Application.Services.Abstractions;

public interface INotesService
{
    public void ConfigureNotes(IEnumerable<Lesson> schedule, string group);
    public Task AddNoteAsync(NoteEntity note);
    public Task RemoveNote(NoteEntity note);
    public NoteEntity? GetNoteById(Guid id);
}