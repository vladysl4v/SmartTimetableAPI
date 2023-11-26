using WebTimetable.Application.Models;

namespace WebTimetable.Application.Handlers.Abstractions
{
    public interface IScheduleHandler
    {
        public Task<List<Lesson>> GetSchedule(DateTime date, string group, CancellationToken token);
    }
}
