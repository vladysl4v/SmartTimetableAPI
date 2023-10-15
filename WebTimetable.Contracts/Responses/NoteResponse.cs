namespace WebTimetable.Contracts.Responses;

public class NoteResponse
{
    public required Guid NoteId { get; init; }
    public required Guid LessonId { get; init; }
    public required Guid AuthorId { get; init; }
    public required string AuthorName { get; init; }
    public required string Message { get; init; }
    public required DateTime CreationDate { get; init; }
}