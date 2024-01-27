namespace WebTimetable.Contracts.Requests;

public class TeacherScheduleRequest
{
    /// <summary>
    /// Teacher identifier.
    /// </summary>
    public required string TeacherId { get; init; }

    /// <summary>
    /// Number of outage group, or leave zero to skip.
    /// </summary>
    public required string OutageGroup { get; init; } = string.Empty;

    /// <summary>
    /// The date from which to start the schedule.
    /// </summary>
    public required string Date { get; init; }

}