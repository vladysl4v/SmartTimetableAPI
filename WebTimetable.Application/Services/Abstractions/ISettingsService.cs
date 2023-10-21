namespace WebTimetable.Application.Services.Abstractions;

public interface ISettingsService
{
    public Dictionary<string, string> GetOutageGroups();
    public Task<Dictionary<string, Dictionary<string, string>>> GetFilters(CancellationToken token);
    public Task<Dictionary<string, string>> GetStudyGroups(string faculty, int course, int educForm, CancellationToken token);
}