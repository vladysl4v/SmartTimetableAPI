namespace WebTimetable.Contracts.Requests;

public class StudyGroupsRequest
{
    /// <summary>
    /// Identifier of the faculty.
    /// </summary>
    public required string Faculty { get; init; }

    /// <summary>
    /// Identifier of the education form.
    /// </summary>
    public required int EducationForm { get; init; }

    /// <summary>
    /// Identifier of the course.
    /// </summary>
    public required int Course { get; init; }
}