using WebTimetable.Application.Entities;
using WebTimetable.Application.Handlers.Abstractions;
using WebTimetable.Application.Services.Abstractions;
using WebTimetable.Application.Models;


namespace WebTimetable.Application.Services;

public class StudentService : IStudentService
{
    private readonly INotesHandler _notes;
    private readonly IEventsHandler _events;
    private readonly IOutagesHandler _outages;
    private readonly IRequestHandler _schedule;

    public StudentService(IEventsHandler events,
        IRequestHandler schedule,
        IOutagesHandler outages,
        INotesHandler notes)
    {
        _schedule = schedule;
        _outages = outages;
        _events = events;
        _notes = notes;
    }

    public async Task<List<StudentLesson>> GetScheduleAsync(DateTime date, string studyGroup, string outageGroup, CancellationToken token, UserEntity? user = null)
    {
        var lessons = await _schedule.GetStudentSchedule(date, studyGroup, token);
        
        if (outageGroup != string.Empty)
        {
            await _outages.ConfigureOutagesAsync(lessons, outageGroup, token);
        }
        if (user is null)
        {
            return lessons;
        }
        _notes.ConfigureNotes(lessons, user.Group);
        await _events.ConfigureEventsAsync(lessons, token);
        return lessons;
    }

    public Task<Dictionary<string, List<KeyValuePair<string, string>>>> GetFiltersAsync(CancellationToken token)
    {
        return _schedule.GetStudentFilters(token);
    }

    public Task<List<KeyValuePair<string, string>>> GetStudyGroupsAsync(string faculty, int course, int educForm, CancellationToken token)
    {
        return _schedule.GetStudentStudyGroups(faculty, course, educForm, token);
    }
    
}