using Microsoft.Graph.Models;

namespace WebTimetable.Application.Services.Abstractions
{
    public interface IEventsService
    {
        public List<Event> GetEvents(DateOnly date, TimeOnly startTime, TimeOnly endTime);
    }
}
