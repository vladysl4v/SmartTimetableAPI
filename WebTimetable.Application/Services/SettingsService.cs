using WebTimetable.Application.Entities;
using WebTimetable.Application.Repositories;
using WebTimetable.Application.Services.Abstractions;


namespace WebTimetable.Application.Services;

public class SettingsService : ISettingsService
{
    private readonly IRepository<OutageEntity> _outages;
    public SettingsService(IRepository<OutageEntity> outages)
    {
        _outages = outages;
    }
    
    public List<KeyValuePair<string, string>> GetOutageGroups()
    {
        return _outages.Where(x => x.City == "Kyiv").Select(y => y.Group)
            .Distinct().ToDictionary(key => key, value => value).ToList();
    }
}