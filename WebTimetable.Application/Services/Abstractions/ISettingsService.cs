namespace WebTimetable.Application.Services.Abstractions;

public interface ISettingsService
{
    public Task<Dictionary<string, Dictionary<string, string>>> GetFilters();
    public Task<Dictionary<string, string>> GetStudyGroups(string faculty, int course, int educForm);
}