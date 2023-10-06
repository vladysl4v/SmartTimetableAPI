using WebTimetable.Application.Models;
using WebTimetable.Application.Schedules.Exceptions;


namespace WebTimetable.Application.Schedules.Abstractions
{
    public interface IScheduleSource
    {
        /// <exception cref="ScheduleNotLoadedException"></exception>
        public Task<List<Lesson>> GetSchedule(DateTime startDate, DateTime endDate, string group, CancellationToken token);
    }
}
