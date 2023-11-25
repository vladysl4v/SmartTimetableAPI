using WebTimetable.Application.Models;


namespace WebTimetable.Application.Handlers.Schedule
{
    public interface IScheduleHandler
    {
        public Task<List<Lesson>> GetSchedule(DateTime date, string group, CancellationToken token);
    }
}
