using WebTimetable.Application.Models;

namespace WebTimetable.Application.Handlers.Abstractions
{
    public interface IEventsHandler
    {
        public Task ConfigureEvents(IEnumerable<Lesson> schedule);
    }
}
