namespace WebTimetable.Contracts.Requests;

public class ScheduleRequest
{
    /// <summary>
    /// Main identifier of person, such as study group for students, or teacherId for teachers.
    /// </summary>
    public required string Identifier { get; init; }

    /// <summary>
    /// Date for which to get the schedule.
    /// </summary>
    public required string Date { get; init; }
}