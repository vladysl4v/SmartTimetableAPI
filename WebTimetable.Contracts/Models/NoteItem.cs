namespace WebTimetable.Contracts.Models;

public class NoteItem
{
    public Guid NoteId { get; init; }
    public Guid AuthorId { get; init; }
    public Guid LessonId { get; init; }
    public string Message { get; init; }
    public string AuthorName { get; init; }
    public DateTime CreationDate { get; init; }
}