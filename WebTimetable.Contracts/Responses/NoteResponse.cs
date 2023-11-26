namespace WebTimetable.Contracts.Responses;

public class NoteResponse
{
    public Guid NoteId { get; init; }
    public Guid LessonId { get; init; }
    public Guid AuthorId { get; init; }
    public string AuthorName { get; init; }
    public string Message { get; init; }
    public DateTime CreationDate { get; init; }
}