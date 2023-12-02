using System.Linq.Expressions;

using WebTimetable.Application.Entities;
using WebTimetable.Application.Models;
using WebTimetable.Application.Repositories;
using WebTimetable.Application.Services.Abstractions;


namespace WebTimetable.Application.Services;

public class NotesService : INotesService
{
    private readonly IRepository<NoteEntity> _notes;
    public NotesService(IRepository<NoteEntity> notes)
    {
        _notes = notes;
    }

    public async Task<bool> AddNoteAsync(NoteEntity note, CancellationToken token)
    {
        var noteExists = _notes.Where(x => x.Author.Id == note.Author.Id && x.LessonId == note.LessonId).Any();
        if (noteExists)
        {
            return false;
        }
        await _notes.AddAsync(note, token);
        await _notes.SaveChangesAsync(token);
        return true;
    }

    public NoteEntity? GetNoteById(Guid id)
    {
        Expression<Func<NoteEntity, bool>> expression = entity => entity.NoteId == id;

        return _notes.Where(expression).SingleOrDefault();
    }

    public async Task RemoveNote(NoteEntity note, CancellationToken token)
    {
        _notes.Remove(note);
        await _notes.SaveChangesAsync(token);
    }
}