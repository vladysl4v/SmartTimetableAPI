using System.ComponentModel.DataAnnotations;

namespace WebTimetable.Contracts.Requests;

public class EmployeesRequest
{
    /// <summary>
    /// Identifier of the faculty.
    /// </summary>
    [Required]
    public required string Faculty { get; init; }

    /// <summary>
    /// Identifier of the chair.
    /// </summary>
    [Required]
    public required string Chair { get; init; }
}