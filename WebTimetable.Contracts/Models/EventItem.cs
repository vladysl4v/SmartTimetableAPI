namespace WebTimetable.Contracts.Models;

public class EventItem
{
    public string Title { get; init; }
    public TimeOnly Start { get; init; }
    public TimeOnly End { get; init; }
    public string Link { get; init; }
}