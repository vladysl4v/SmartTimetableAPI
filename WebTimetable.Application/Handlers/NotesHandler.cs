using WebTimetable.Application.Handlers.Abstractions;
using WebTimetable.Application.Models;
using WebTimetable.Application.Repositories.Abstractions;

namespace WebTimetable.Application.Handlers;

public class NotesHandler : INotesHandler
{
    private readonly INotesRepository _notesRepository;
    public NotesHandler(INotesRepository notesRepository)
    {
        _notesRepository = notesRepository;
    }

    public void ConfigureNotes(IEnumerable<StudentLesson> schedule, string userGroup)
    {
        foreach (var lesson in schedule)
        {
            lesson.Notes = _notesRepository.GetNotesByLessonId(lesson.Id, userGroup);
        }
    }
}