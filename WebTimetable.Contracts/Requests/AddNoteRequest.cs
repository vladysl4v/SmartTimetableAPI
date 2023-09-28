namespace WebTimetable.Contracts.Requests;

public class AddNoteRequest
{
    public required Guid LessonId { get; init; }
    public required string Message { get; init; }
}