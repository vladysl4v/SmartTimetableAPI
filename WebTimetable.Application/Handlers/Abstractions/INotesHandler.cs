using WebTimetable.Application.Models;

namespace WebTimetable.Application.Handlers.Abstractions;

public interface INotesHandler
{
    public void ConfigureNotes(IEnumerable<StudentLesson> schedule, string userGroup, Guid userId);
}