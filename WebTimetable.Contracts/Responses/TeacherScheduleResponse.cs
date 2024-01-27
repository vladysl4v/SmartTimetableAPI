using WebTimetable.Contracts.Models;

namespace WebTimetable.Contracts.Responses;

public class TeacherScheduleResponse
{
    public List<TeacherLessonItem> Schedule { get; init; }
}