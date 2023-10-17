using WebTimetable.Application.Models;
using WebTimetable.Contracts.Models;
using WebTimetable.Contracts.Responses;


namespace WebTimetable.Api.Mapping;

public static class ScheduleMappingExtensions
{
    public static ScheduleResponse MapToScheduleResponse(this IEnumerable<Lesson> schedule, Guid? userId = null)
    {
        var convertedSchedule = schedule.Select(lesson => new LessonItem
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
            }).ToList(),

            Notes = lesson.Notes?.Select(note => new NoteItem
            {
                NoteId = note.NoteId,
                AuthorId = note.Author.Id,
                AuthorName = note.Author.FullName,
                AuthorGroup = note.Author.Group,
                LessonId = note.LessonId,
                Message = note.Message,
                CreationDate = note.CreationDate,
                IsAuthor = userId == note.Author.Id
            }).ToList(),

            Meetings = lesson.Events?.Select(meeting => new EventItem
            {
                Start = meeting.StartTime,
                End = meeting.EndTime,
                Title = meeting.Title,
                Link = meeting.Link
            }).ToList()
        });
        return new ScheduleResponse
        {
            Schedule = convertedSchedule.ToList()
        };
    }
}