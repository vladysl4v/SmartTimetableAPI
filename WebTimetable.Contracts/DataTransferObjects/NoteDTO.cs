namespace WebTimetable.Contracts.DataTransferObjects;

public class NoteDTO
{
    public Guid NoteId { get; init; }
    public Guid AuthorId { get; init; }
    public Guid LessonId { get; init; }
    public string Message { get; init; }
    public string AuthorName { get; init; }
    public DateTime CreationDate { get; init; }
}