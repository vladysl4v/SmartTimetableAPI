using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebTimetable.Contracts.Requests;

public class StudyGroupsRequest
{
    /// <summary>
    /// Identifier of the faculty. Ignorable.
    /// </summary>
    [Required(AllowEmptyStrings = true)]
    [JsonPropertyName("facultyID")]
    public required string Faculty { get; init; }

    /// <summary>
    /// Identifier of the education form. Ignorable.
    /// </summary>
    [Required(AllowEmptyStrings = true)]
    [JsonPropertyName("educFormID")]
    public required int EducationForm { get; init; }

    /// <summary>
    /// Identifier of the course. Ignorable.
    /// </summary>
    [Required(AllowEmptyStrings = true)]
    [JsonPropertyName("courseID")]
    public required int Course { get; init; }
}