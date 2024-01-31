using WebTimetable.Application.Entities;
using WebTimetable.Application.Handlers.Abstractions;
using WebTimetable.Application.Models;
using WebTimetable.Application.Services.Abstractions;

namespace WebTimetable.Application.Services;

public class TeacherService : ITeacherService
{
    private readonly IEventsHandler _events;
    private readonly IOutagesHandler _outages;
    private readonly IRequestHandler _schedule;

    public TeacherService(IEventsHandler events,
        IRequestHandler schedule,
        IOutagesHandler outages)
    {
        _schedule = schedule;
        _outages = outages;
        _events = events;
    }

    public async Task<List<TeacherLesson>> GetScheduleAsync(DateTime date, string teacherId, string outageGroup,
        CancellationToken token, UserEntity? user = null)
    {
        var lessons = await _schedule.GetTeacherSchedule(date, teacherId, token);
        
        if (outageGroup != string.Empty)
        {
            await _outages.ConfigureOutagesAsync(lessons, outageGroup, token);
        }
        if (user is null)
        {
            return lessons;
        }
        await _events.ConfigureEventsAsync(lessons, token);
        return lessons;
    }
    
    public Task<List<KeyValuePair<string, string>>> GetFacultiesAsync(CancellationToken token)
    {
        return _schedule.GetTeacherFaculties(token);
    }

    public Task<List<KeyValuePair<string, string>>> GetChairsAsync(string faculty, CancellationToken token)
    {
        return _schedule.GetTeacherChairs(faculty, token);
    }

    public Task<List<KeyValuePair<string, string>>> GetEmployeesAsync(string faculty, string chair,
        CancellationToken token)
    {
        return _schedule.GetTeacherEmployees(faculty, chair, token);
    }
}