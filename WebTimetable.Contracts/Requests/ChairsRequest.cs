using System.ComponentModel.DataAnnotations;

namespace WebTimetable.Contracts.Requests;

public class ChairsRequest
{
    /// <summary>
    /// Identifier of the faculty.
    /// </summary>
    [Required]
    public required string Faculty { get; init; }
}