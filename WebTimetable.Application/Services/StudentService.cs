using WebTimetable.Application.Handlers.Abstractions;
using WebTimetable.Application.Services.Abstractions;
using WebTimetable.Application.Models;
using WebTimetable.Application.Repositories.Abstractions;


namespace WebTimetable.Application.Services;

public class StudentService : IStudentService
{
    private readonly INotesRepository _notes;
    private readonly IEventsHandler _events;
    private readonly IOutagesHandler _outages;
    private readonly IRequestHandler _requests;

    public StudentService(IEventsHandler events,
        IRequestHandler requests,
        IOutagesHandler outages,
        INotesRepository notes)
    {
        _requests = requests;
        _outages = outages;
        _events = events;
        _notes = notes;
    }

    public async Task<List<StudentLesson>> GetScheduleAsync(DateTime date, string studyGroup, string outageGroup, CancellationToken token)
    {
        var lessons = await _requests.GetStudentSchedule(date, studyGroup, token);
        
        if (outageGroup != string.Empty)
        {
            await _outages.ConfigureOutagesAsync(lessons, outageGroup, token);
        }

        return lessons;
    }
    
    public async Task<LessonDetails> GetLessonDetails(Guid id, DateOnly date, TimeOnly lessonStart, TimeOnly lessonEnd, string userGroup, CancellationToken token)
    {
        return new LessonDetails
        {
            Id = id,
            Events = await _events.GetEventsAsync(date, lessonStart, lessonEnd, token),
            Notes = _notes.GetNotesByLessonId(id, userGroup)
        };
    }

    public Task<Dictionary<string, List<KeyValuePair<string, string>>>> GetFiltersAsync(CancellationToken token)
    {
        return _requests.GetStudentFilters(token);
    }

    public Task<List<KeyValuePair<string, string>>> GetStudyGroupsAsync(string faculty, int course, int educForm, CancellationToken token)
    {
        return _requests.GetStudentStudyGroups(faculty, course, educForm, token);
    }
    
}