using WebTimetable.Application.Models;


namespace WebTimetable.Application.Handlers.Schedule
{
    public interface IScheduleHandler
    {
        public Task<List<Lesson>> GetSchedule(DateTime startDate, DateTime endDate, string group, CancellationToken token);
    }
}
