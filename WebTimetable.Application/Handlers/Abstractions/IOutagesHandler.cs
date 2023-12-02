using WebTimetable.Application.Models;

namespace WebTimetable.Application.Handlers.Abstractions
{
    public interface IOutagesHandler
    {
        public Task ConfigureOutagesAsync(IEnumerable<Lesson> schedule, string outageGroup, string city, CancellationToken token);
    }
}