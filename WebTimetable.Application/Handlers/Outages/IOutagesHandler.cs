using WebTimetable.Application.Models;


namespace WebTimetable.Application.Handlers.Outages
{
    public interface IOutagesHandler
    {
        public List<string> GetOutageGroups(string city);
        public Task ConfigureOutagesAsync(IEnumerable<Lesson> schedule, string outageGroup, string city);
    }
}
