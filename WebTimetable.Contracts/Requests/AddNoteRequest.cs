namespace WebTimetable.Contracts.Requests;

public class AddNoteRequest
{
    /// <summary>
    /// Identifier of the lesson.
    /// </summary>
    public required Guid LessonId { get; init; }

    /// <summary>
    /// Message that needs to be included in the note.
    /// </summary>
    public required string Message { get; init; }
}