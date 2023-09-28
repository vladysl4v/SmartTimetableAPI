using WebTimetable.Application.Models;
using WebTimetable.Contracts.Models;
using WebTimetable.Contracts.Responses;


namespace WebTimetable.Api.Mapping;

public static class ScheduleMappingExtensions
{
    public static AnonymousScheduleResponse MapToAnonymousScheduleResponse(this IEnumerable<Lesson> schedule)
    {
        var convertedSchedule = schedule.Select(lesson => new AnonymousLessonItem
        {
            Id = lesson.Id,
            Date = lesson.Date,
            Start = lesson.Start,
            End = lesson.End,
            Discipline = lesson.Discipline,
            StudyType = lesson.StudyType,
            Cabinet = lesson.Cabinet,
            Teacher = lesson.Teacher,
            Subgroup = lesson.Subgroup,
            Outages = lesson.Outages.Select(outage => new OutageItem
            {
                IsDefinite = outage.IsDefinite.Value,
                Start = outage.Start,
                End = outage.End
            }).ToList()
        });
        return new AnonymousScheduleResponse
        {
            Schedule = convertedSchedule.ToList()
        };
    }

    public static PersonalScheduleResponse MapToPersonalScheduleResponse(this IEnumerable<Lesson> schedule)
    {
        var convertedSchedule = schedule.Select(lesson => new PersonalLessonItem
        {
            Id = lesson.Id,
            Date = lesson.Date,
            Start = lesson.Start,
            End = lesson.End,
            Discipline = lesson.Discipline,
            StudyType = lesson.StudyType,
            Cabinet = lesson.Cabinet,
            Teacher = lesson.Teacher,
            Subgroup = lesson.Subgroup,
            Outages = lesson.Outages.Select(outage => new OutageItem
            {
                IsDefinite = outage.IsDefinite.Value,
                Start = outage.Start,
                End = outage.End
            }).ToList()
        });
        return new PersonalScheduleResponse
        {
            Schedule = convertedSchedule.ToList()
        };
    }
}