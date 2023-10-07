using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebTimetable.Contracts.Requests;

public class StudyGroupsRequest
{
    /// <summary>
    /// Identifier of the faculty. Ignorable.
    /// </summary>
    public required string Faculty { get; init; }

    /// <summary>
    /// Identifier of the education form. Ignorable.
    /// </summary>
    public required int EducationForm { get; init; }

    /// <summary>
    /// Identifier of the course. Ignorable.
    /// </summary>
    public required int Course { get; init; }
}