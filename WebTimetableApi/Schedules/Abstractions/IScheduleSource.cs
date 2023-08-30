using WebTimetableApi.Models;
using WebTimetableApi.Schedules.Exceptions;


namespace WebTimetableApi.Schedules.Abstractions
{
    public interface IScheduleSource
    {
        /// <exception cref="ScheduleNotLoadedException"></exception>
        public Task<List<Lesson>> GetSchedule(DateTime date, string group);
    }
}
