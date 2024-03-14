using System.ComponentModel.DataAnnotations;

namespace WebTimetable.Contracts.Requests;

public class ScheduleRequest
{
    /// <summary>
    /// Main identifier of person, such as study group for students, or teacherId for teachers.
    /// </summary>
    [Required]
    public required string Identifier { get; init; }

    /// <summary>
    /// Date for which to get the schedule.
    /// </summary>
    [Required]
    public required string Date { get; init; }
}