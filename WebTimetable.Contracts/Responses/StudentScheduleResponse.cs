using WebTimetable.Contracts.DataTransferObjects;

namespace WebTimetable.Contracts.Responses;

public class StudentScheduleResponse
{
    public List<StudentLessonDTO> Schedule { get; init; }
}