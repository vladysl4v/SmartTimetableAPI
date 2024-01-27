namespace WebTimetable.Contracts.Requests;

public class EmployeesRequest
{
    /// <summary>
    /// Identifier of the faculty.
    /// </summary>
    public required string Faculty { get; init; }

    /// <summary>
    /// Identifier of the chair.
    /// </summary>
    public required string Chair { get; init; }
}