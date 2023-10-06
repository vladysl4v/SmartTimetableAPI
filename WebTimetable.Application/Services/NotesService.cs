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

    public async Task AddNoteAsync(NoteEntity note, CancellationToken token)
    {
        await _dbRepository.Add(note);
        await _dbRepository.SaveChangesAsync(token);
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
            entity.LessonId == lessonId && entity.AuthorGroup == group;

        return _dbRepository.Get<NoteEntity>(expression).ToList();
    }
}