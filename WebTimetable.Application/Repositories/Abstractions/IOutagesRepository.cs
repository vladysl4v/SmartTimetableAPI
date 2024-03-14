using WebTimetable.Application.Models;

namespace WebTimetable.Application.Repositories.Abstractions;

public interface IOutagesRepository
{
    public Task<List<Outage>?> GetOutagesByDayOfWeekAsync(DayOfWeek dayOfWeek, string outageGroup, CancellationToken token);
    public Task AddOutagesAsync(Dictionary<int, Dictionary<DayOfWeek, List<Outage>>> outages,
        Dictionary<string, string> outageGroups, CancellationToken token);
    public List<KeyValuePair<string, string>> GetOutageGroups(string city);

}