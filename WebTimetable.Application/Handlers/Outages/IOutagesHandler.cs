using WebTimetable.Application.Models;


namespace WebTimetable.Application.Handlers.Outages
{
    public interface IOutagesHandler
    {
        public Task InitializeOutages();
        public Dictionary<string, string> GetOutageGroups();
        public void ConfigureOutages(IEnumerable<Lesson> schedule, int outageGroup);
    }
}
