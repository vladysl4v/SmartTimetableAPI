using System.Linq.Expressions;

using WebTimetable.Application.Entities;
using WebTimetable.Application.Models;
using WebTimetable.Application.Repositories;
using WebTimetable.Application.Services.Abstractions;


namespace WebTimetable.Application.Services;

public class NotesService : INotesService
{
    private readonly IDbRepository _dbRepository;
    public NotesService(IDbRepository dbRepository)
    {
        _dbRepository = dbRepository;
    }

    public async Task<bool> AddNoteAsync(NoteEntity note, CancellationToken token)
    {
        var noteExists = _dbRepository.Get<NoteEntity>(x => x.Author.Id == note.Author.Id && x.LessonId == note.LessonId).Any();
        if (noteExists)
        {
            return false;
        }
        await _dbRepository.Add(note);
        await _dbRepository.SaveChangesAsync(token);
        return true;
    }

    public void ConfigureNotes(IEnumerable<Lesson> schedule, string group)
    {
        foreach (var lesson in schedule)
        {
            lesson.Notes = GetNotesByLessonId(lesson.Id, group);
        }
    }
    public NoteEntity? GetNoteById(Guid id)
    {
        Expression<Func<NoteEntity, bool>> expression = entity => entity.NoteId == id;

        return _dbRepository.Get<NoteEntity>(expression).SingleOrDefault();
    }

    public async Task RemoveNote(NoteEntity note, CancellationToken token)
    {
        _dbRepository.Remove(note);
        await _dbRepository.SaveChangesAsync(token);
    }

    private List<NoteEntity> GetNotesByLessonId(Guid lessonId, string group)
    {
        Expression<Func<NoteEntity, bool>> expression = entity =>
            entity.LessonId == lessonId && entity.Author.Group == group;

        return _dbRepository.Get<NoteEntity>(expression).ToList();
    }
}