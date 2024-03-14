using System.ComponentModel.DataAnnotations;

namespace WebTimetable.Contracts.Requests;

public class AddNoteRequest
{
    /// <summary>
    /// Identifier of the lesson.
    /// </summary>
    [Required]
    public required Guid LessonId { get; init; }

    /// <summary>
    /// Message that needs to be included in the note.
    /// </summary>
    [Required]
    public required string Message { get; init; }
}