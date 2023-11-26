using System.Linq.Expressions;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Handlers.Abstractions;
using WebTimetable.Application.Models;
using WebTimetable.Application.Repositories;

namespace WebTimetable.Application.Handlers;

public class NotesHandler : INotesHandler
{
    private readonly IDbRepository _dbRepository;
    public NotesHandler(IDbRepository dbRepository)
    {
        _dbRepository = dbRepository;
    }

    public void ConfigureNotes(IEnumerable<Lesson> schedule, string userGroup, Guid userId)
    {
        foreach (var lesson in schedule)
        {
            lesson.Notes = GetNotesByLessonId(lesson.Id, userGroup, userId);
        }
    }

    private List<Note> GetNotesByLessonId(Guid lessonId, string userGroup, Guid userId)
    {
        Expression<Func<NoteEntity, bool>> expression = entity =>
            entity.LessonId == lessonId && entity.Author.Group == userGroup;

        return _dbRepository.Get<NoteEntity>(expression).Select(x => new Note
        {
            AuthorId = x.AuthorId,
            CreationDate = x.CreationDate,
            LessonId = x.LessonId,
            NoteId = x.NoteId,
            Message = x.Message,
            AuthorName = x.Author.FullName,
        }).ToList();
    }
}