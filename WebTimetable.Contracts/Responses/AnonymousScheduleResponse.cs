using WebTimetable.Contracts.Models;


namespace WebTimetable.Contracts.Responses;


public class AnonymousScheduleResponse
{
    public required List<AnonymousLessonItem> Schedule { get; init; }
}