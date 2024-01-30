using WebTimetable.Contracts.DataTransferObjects;

namespace WebTimetable.Contracts.Responses;

public class TeacherScheduleResponse
{
    public List<TeacherLessonDTO> Schedule { get; init; }
}