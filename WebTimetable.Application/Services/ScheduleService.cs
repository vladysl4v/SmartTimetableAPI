﻿using WebTimetable.Application.Entities;
using WebTimetable.Application.Handlers.Abstractions;
using WebTimetable.Application.Services.Abstractions;
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
        INotesHandler notes)
    {
        _schedule = schedule;
        _outages = outages;
        _events = events;
        _notes = notes;
    }

    public async Task<List<Lesson>?> GetPersonalSchedule(DateTime date, string studyGroup, string outageGroup, UserEntity? user, CancellationToken token)
    {
        if (user is null)
        {
            return null;
        }
        var lessons = await _schedule.GetSchedule(date, studyGroup, token);

        _notes.ConfigureNotes(lessons, user.Group, user.Id);
        await _events.ConfigureEvents(lessons);
        if (outageGroup != string.Empty)
        {
            await _outages.ConfigureOutagesAsync(lessons, outageGroup, "Kyiv", token);
        }
        
        return lessons;
    }

    public async Task<List<Lesson>> GetGuestSchedule(DateTime date, string studyGroup, string outageGroup, CancellationToken token)
    {
        var lessons = await _schedule.GetSchedule(date, studyGroup, token);
        if (outageGroup != string.Empty)
        {
            await _outages.ConfigureOutagesAsync(lessons, outageGroup, "Kyiv", token);
        }

        return lessons;
    }
}