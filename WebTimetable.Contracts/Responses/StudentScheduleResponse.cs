using WebTimetable.Contracts.Models;

namespace WebTimetable.Contracts.Responses;

public class StudentScheduleResponse
{
    public List<StudentLessonItem> Schedule { get; init; }
}