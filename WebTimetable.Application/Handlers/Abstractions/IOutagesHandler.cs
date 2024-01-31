using WebTimetable.Application.Models;
using WebTimetable.Application.Models.Abstractions;

namespace WebTimetable.Application.Handlers.Abstractions
{
    public interface IOutagesHandler
    {
        public Task ConfigureOutagesAsync(IEnumerable<ILesson> schedule, string outageGroup, CancellationToken token);
    }
}