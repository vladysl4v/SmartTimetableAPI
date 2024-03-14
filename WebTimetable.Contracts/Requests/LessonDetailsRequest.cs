namespace WebTimetable.Contracts.Requests;

public class LessonDetailsRequest
{
    public required string Date { get; set; }
    public required string StartTime { get; set; }
    public required string EndTime { get; set; }
}