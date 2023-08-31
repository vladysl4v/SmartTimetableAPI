using WebTimetableApi.Models;


namespace WebTimetableApi.Handlers.Abstractions
{
    public interface IOutagesHandler
    {
        public Task InitializeOutages();
        public List<Outage> GetOutages(TimeOnly startTime, TimeOnly endTime, DayOfWeek dayOfWeek);
    }
}
