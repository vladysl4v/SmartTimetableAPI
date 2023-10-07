using WebTimetable.Application.Models;


namespace WebTimetable.Application.Schedules.Abstractions
{
    public interface IScheduleSource
    {
        public Task<List<Lesson>> GetSchedule(DateTime startDate, DateTime endDate, string group, CancellationToken token);
    }
}
