namespace WebTimetable.Contracts.Models;

public class EventItem
{
    public required string Title { get; init; }
    public required TimeOnly Start { get; init; }
    public required TimeOnly End { get; init; }
    public required string Link { get; init; }
}