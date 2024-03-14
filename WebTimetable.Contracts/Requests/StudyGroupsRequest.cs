using System.ComponentModel.DataAnnotations;

namespace WebTimetable.Contracts.Requests;

public class StudyGroupsRequest
{
    /// <summary>
    /// Identifier of the faculty.
    /// </summary>
    [Required]
    public required string Faculty { get; init; }

    /// <summary>
    /// Identifier of the education form.
    /// </summary>
    [Required]
    public required int EducationForm { get; init; }

    /// <summary>
    /// Identifier of the course.
    /// </summary>
    [Required]
    public required int Course { get; init; }
}