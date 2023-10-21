using WebTimetable.Application.Models;


namespace WebTimetable.Application.Handlers.Events
{
    public interface IEventsHandler
    {
        public Task ConfigureEvents(IEnumerable<Lesson> schedule);
    }
}
