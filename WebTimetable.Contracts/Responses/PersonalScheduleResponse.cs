using WebTimetable.Contracts.Models;

namespace WebTimetable.Contracts.Responses;

public class PersonalScheduleResponse
{
    public required List<PersonalLessonItem> Schedule { get; init; }
}