using WebTimetable.Application.Entities;
using WebTimetable.Application.Repositories;
using WebTimetable.Application.Repositories.Abstractions;
using WebTimetable.Application.Services.Abstractions;


namespace WebTimetable.Application.Services;

public class SettingsService : ISettingsService
{
    private readonly IOutagesRepository _outagesRepository;
    public SettingsService(IOutagesRepository outagesRepository)
    {
        _outagesRepository = outagesRepository;
    }
    
    public List<KeyValuePair<string, string>> GetOutageGroups(string city)
    {
        return _outagesRepository.GetOutageGroups(city);
    }
}