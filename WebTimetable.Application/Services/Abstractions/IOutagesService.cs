using WebTimetable.Application.Models;


namespace WebTimetable.Application.Services.Abstractions
{
    public interface IOutagesService
    {
        public Task InitializeOutages();
        public Dictionary<string, string> GetOutageGroups();
        public void ConfigureOutages(IEnumerable<Lesson> schedule, int outageGroup);
    }
}
