using WebTimetable.Application.Entities;
using WebTimetable.Application.Repositories.Abstractions;
using WebTimetable.Application.Services.Abstractions;

namespace WebTimetable.Application.Services;

public class NotesService : INotesService
{
    private readonly INotesRepository _notesRepository;
    public NotesService(INotesRepository notesRepository)
    {
        _notesRepository = notesRepository;
    }

    public async Task<bool> AddNoteAsync(NoteEntity note, CancellationToken token)
    {
        if (_notesRepository.IsNoteExists(note.Author.Id, note.LessonId))
        {
            return false;
        }
        
        await _notesRepository.AddAsync(note, token);
        return true;
    }

    public NoteEntity? GetNoteById(Guid id)
    {
        return _notesRepository.GetNoteById(id);
    }

    public async Task RemoveNote(NoteEntity note, CancellationToken token)
    {
        await _notesRepository.RemoveAsync(note, token);
    }
}