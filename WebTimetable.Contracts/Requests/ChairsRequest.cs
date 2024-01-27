namespace WebTimetable.Contracts.Requests;

public class ChairsRequest
{
    /// <summary>
    /// Identifier of the faculty.
    /// </summary>
    public required string Faculty { get; init; }
}