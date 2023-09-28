namespace WebTimetable.Contracts.Models;

public class OutageItem
{
    public required bool IsDefinite { get; init; }
    public required TimeOnly Start { get; init; }
    public required TimeOnly End { get; init; }
}