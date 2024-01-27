using WebTimetable.Application.Entities;
using WebTimetable.Application.Models;

namespace WebTimetable.Application.Services.Abstractions;

public interface ITeacherService
{
    public Task<List<TeacherLesson>> GetScheduleAsync(DateTime date, string teacherId, string outageGroup, CancellationToken token, UserEntity? user = null);
    
    public Task<List<KeyValuePair<string, string>>> GetFacultiesAsync(CancellationToken token);

    public Task<List<KeyValuePair<string, string>>> GetChairsAsync(string faculty, CancellationToken token);

    public Task<List<KeyValuePair<string, string>>> GetEmployeesAsync(string faculty, string chair, CancellationToken token);
}