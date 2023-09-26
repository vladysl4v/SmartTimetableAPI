using WebTimetable.Application.Models;

namespace WebTimetable.Application.Services.Abstractions
{
    public interface IEventsService
    {
        public void ConfigureEvents(IEnumerable<Lesson> schedule);
    }
}
