using WebTimetable.Contracts.Models;

namespace WebTimetable.Contracts.Responses;

public class ScheduleResponse
{
    public required List<LessonItem> Schedule { get; init; }
}