using WebTimetable.Application.Models;
using WebTimetable.Application.Models.Abstractions;

namespace WebTimetable.Application.Handlers.Abstractions
{
    public interface IEventsHandler
    {
        public Task<List<Event>> GetEventsAsync(DateOnly date, TimeOnly start, TimeOnly end, CancellationToken token);
    }
}