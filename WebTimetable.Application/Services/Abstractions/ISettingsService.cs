namespace WebTimetable.Application.Services.Abstractions;

public interface ISettingsService
{
    public List<KeyValuePair<string, string>> GetOutageGroups(string city);
}