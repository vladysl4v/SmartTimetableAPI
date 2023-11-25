namespace WebTimetable.Application.Models;

public class Note
{
    public Guid NoteId { get; set; }
    public Guid LessonId { get; set; }
    public string Message { get; set; }
    public Guid AuthorId { get; set; }
    public string AuthorName { get; set; }
    public DateTime CreationDate { get; set; }
}