using WebTimetable.Application.Models;

namespace WebTimetable.Application.Handlers.Notes;

public interface INotesHandler
{
    public void ConfigureNotes(IEnumerable<Lesson> schedule, string userGroup, Guid userId);
}