using System.ComponentModel.DataAnnotations;

namespace WebTimetable.Contracts.Requests;

public class LessonDetailsRequest
{
    /// <summary>
    /// Date of lesson.
    /// </summary>
    [Required]
    public required string Date { get; set; }
    
    /// <summary>
    /// Start time of lesson.
    /// </summary>
    [Required]
    public required string StartTime { get; set; }
    
    /// <summary>
    /// End time of lesson.
    /// </summary>
    [Required]
    public required string EndTime { get; set; }
}