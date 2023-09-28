namespace WebTimetable.Contracts.Models;

public class AnonymousLessonItem
{
    public required Guid Id { get; init; }
    public required DateOnly Date { get; init; }
    public required TimeOnly Start { get; init; }
    public required TimeOnly End { get; init; }
    public required string Discipline { get; init; }
    public required string StudyType { get; init; }
    public required string Cabinet { get; init; }
    public required string Teacher { get; init; }
    public required string Subgroup { get; init; }
    public required List<OutageItem> Outages { get; init; }
}