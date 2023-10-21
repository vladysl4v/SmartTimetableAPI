using WebTimetable.Application.Entities;
using WebTimetable.Application.Services.Abstractions;
using WebTimetable.Application.Handlers.Schedule;
using WebTimetable.Application.Handlers.Outages;
using WebTimetable.Application.Handlers.Events;
using WebTimetable.Application.Handlers.Notes;
using WebTimetable.Application.Models;


namespace WebTimetable.Application.Services;

public class ScheduleService : IScheduleService
{
    private readonly INotesHandler _notes;
    private readonly IEventsHandler _events;
    private readonly IOutagesHandler _outages;
    private readonly IScheduleHandler _schedule;

    public ScheduleService(IEventsHandler events,
        IScheduleHandler schedule,
        IOutagesHandler outages,
        INotesHandler notes,
        IUsersService users)
    {
        _schedule = schedule;
        _outages = outages;
        _events = events;
        _notes = notes;
    }

    public async Task<List<Lesson>?> GetPersonalSchedule(DateTime start, DateTime end, string studyGroup, int outageGroup, UserEntity? user, CancellationToken token)
    {
        if (user is null)
        {
            return null;
        }
        var lessons = await _schedule.GetSchedule(start, end, studyGroup, token);

        _notes.ConfigureNotes(lessons, user.Group, user.Id);
        await _events.ConfigureEvents(lessons);
        if (outageGroup != 0)
        {
            _outages.ConfigureOutages(lessons, outageGroup);
        }
        
        return lessons;
    }

    public async Task<List<Lesson>> GetGuestSchedule(DateTime start, DateTime end, string studyGroup, int outageGroup, CancellationToken token)
    {
        var lessons = await _schedule.GetSchedule(start, end, studyGroup, token);
        if (outageGroup != 0)
        {
            _outages.ConfigureOutages(lessons, outageGroup);
        }

        return lessons;
    }
}