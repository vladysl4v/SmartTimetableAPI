﻿namespace WebTimetable.Contracts.Requests;

public class AnonymousScheduleRequest
{
    /// <summary>
    /// Identifier of user study group.
    /// </summary>
    public required string StudyGroup { get; init; }

    /// <summary>
    /// Number of outage group, or leave empty to skip.
    /// </summary>
    public required string OutageGroup { get; init; } = string.Empty;

    /// <summary>
    /// The date from which to start the schedule.
    /// </summary>
    public required string Date { get; init; }
    
}