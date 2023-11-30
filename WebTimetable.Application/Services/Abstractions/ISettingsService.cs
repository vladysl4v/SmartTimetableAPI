namespace WebTimetable.Application.Services.Abstractions;

public interface ISettingsService
{
    public Task<Dictionary<string, List<KeyValuePair<string, string>>>> GetFilters(CancellationToken token);
    public Task<List<KeyValuePair<string, string>>> GetStudyGroups(string faculty, int course, int educForm, CancellationToken token);
}