namespace WebTimetable.Contracts.Models;

public class LessonItem
{
    public Guid Id { get; init; }
    public DateOnly Date { get; init; }
    public TimeOnly Start { get; init; }
    public TimeOnly End { get; init; }
    public string Discipline { get; init; }
    public string StudyType { get; init; }
    public string Cabinet { get; init; }
    public string Teacher { get; init; }
    public string Subgroup { get; init; }
    public List<EventItem>? Meetings { get; set; }
    public List<NoteItem>? Notes { get; init; }
    public List<OutageItem> Outages { get; init; }
}