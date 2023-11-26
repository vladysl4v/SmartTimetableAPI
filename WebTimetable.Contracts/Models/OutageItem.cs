namespace WebTimetable.Contracts.Models;

public class OutageItem
{
    public bool IsDefinite { get; init; }
    public TimeOnly Start { get; init; }
    public TimeOnly End { get; init; }
}