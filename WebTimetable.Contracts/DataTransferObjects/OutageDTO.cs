namespace WebTimetable.Contracts.DataTransferObjects;

public class OutageDTO
{
    public bool IsDefinite { get; init; }
    public TimeOnly Start { get; init; }
    public TimeOnly End { get; init; }
}