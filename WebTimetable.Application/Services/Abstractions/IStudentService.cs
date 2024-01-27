using WebTimetable.Application.Entities;
using WebTimetable.Application.Models;


namespace WebTimetable.Application.Services.Abstractions;

public interface IStudentService
{
    public Task<List<StudentLesson>> GetScheduleAsync(DateTime date, string studyGroup, string outageGroup,
        CancellationToken token, UserEntity? user = null);
    
    public Task<Dictionary<string, List<KeyValuePair<string, string>>>> GetFiltersAsync(CancellationToken token);
    
    public Task<List<KeyValuePair<string, string>>> GetStudyGroupsAsync(string faculty, int course, int educForm, CancellationToken token);
}