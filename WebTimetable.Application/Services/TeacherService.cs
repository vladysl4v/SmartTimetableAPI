using WebTimetable.Application.Handlers.Abstractions;
using WebTimetable.Application.Models;
using WebTimetable.Application.Repositories.Abstractions;
using WebTimetable.Application.Services.Abstractions;

namespace WebTimetable.Application.Services;

public class TeacherService : ITeacherService
{
    private readonly INotesRepository _notes;
    private readonly IEventsHandler _events;
    private readonly IOutagesHandler _outages;
    private readonly IRequestHandler _requests;

    public TeacherService(IEventsHandler events,
        IRequestHandler requests,
        IOutagesHandler outages,
        INotesRepository notes)
    {
        _requests = requests;
        _outages = outages;
        _events = events;
        _notes = notes;
    }

    public async Task<List<TeacherLesson>> GetScheduleAsync(DateTime date, string teacherId, string outageGroup,
        CancellationToken token)
    {
        var lessons = await _requests.GetTeacherSchedule(date, teacherId, token);
        
        if (outageGroup != string.Empty)
        {
            await _outages.ConfigureOutagesAsync(lessons, outageGroup, token);
        }

        return lessons;
    }
    
    // TODO refactor this shit
    public async Task<LessonDetails> GetLessonDetails(Guid id, DateOnly date, TimeOnly lessonStart, TimeOnly lessonEnd, string userGroup, CancellationToken token)
    {
        return new LessonDetails
        {
            Id = id,
            Events = await _events.GetEventsAsync(date, lessonStart, lessonEnd, token),
            Notes = _notes.GetNotesByLessonId(id, userGroup)
        };
    }
    
    public Task<List<KeyValuePair<string, string>>> GetFacultiesAsync(CancellationToken token)
    {
        return _requests.GetTeacherFaculties(token);
    }

    public Task<List<KeyValuePair<string, string>>> GetChairsAsync(string faculty, CancellationToken token)
    {
        return _requests.GetTeacherChairs(faculty, token);
    }

    public Task<List<KeyValuePair<string, string>>> GetEmployeesAsync(string faculty, string chair,
        CancellationToken token)
    {
        return _requests.GetTeacherEmployees(faculty, chair, token);
    }
}