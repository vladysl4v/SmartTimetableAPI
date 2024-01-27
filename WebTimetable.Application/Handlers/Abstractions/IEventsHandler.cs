using WebTimetable.Application.Models;
using WebTimetable.Application.Models.Abstractions;

namespace WebTimetable.Application.Handlers.Abstractions
{
    public interface IEventsHandler
    {
        public Task ConfigureEventsAsync(IEnumerable<ILesson> lessons, CancellationToken token);
    }
}