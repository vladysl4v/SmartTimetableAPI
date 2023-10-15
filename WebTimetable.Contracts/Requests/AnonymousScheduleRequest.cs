namespace WebTimetable.Contracts.Requests;

public class AnonymousScheduleRequest
{
    /// <summary>
    /// Identifier of user study group.
    /// </summary>
    public required string StudyGroup { get; init; }

    /// <summary>
    /// Number of outage group, or leave zero to skip.
    /// </summary>
    public required int OutageGroup { get; init; }

    /// <summary>
    /// The date from which to start the schedule.
    /// </summary>
    public required string StartDate { get; init; }

    /// <summary>
    /// The date until which to look for the schedule.
    /// </summary>
    public required string EndDate { get; init; }
}