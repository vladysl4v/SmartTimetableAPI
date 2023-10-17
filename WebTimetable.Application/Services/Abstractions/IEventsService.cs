using WebTimetable.Application.Models;

namespace WebTimetable.Application.Services.Abstractions
{
    public interface IEventsService
    {
        public Task ConfigureEvents(IEnumerable<Lesson> schedule);
    }
}
