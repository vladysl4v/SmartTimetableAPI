using WebTimetable.Application.Models;


namespace WebTimetable.Application.Services.Abstractions
{
    public interface IOutagesService
    {
        public Task InitializeOutages();
        public List<Outage> GetOutages(TimeOnly startTime, TimeOnly endTime, DayOfWeek dayOfWeek, int outageGroup);
    }
}
