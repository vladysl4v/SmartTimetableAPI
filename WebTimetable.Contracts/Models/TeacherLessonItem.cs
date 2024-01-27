namespace WebTimetable.Contracts.Models;

public class TeacherLessonItem
{
    public Guid Id { get; init; }
    public DateOnly Date { get; init; }
    public TimeOnly Start { get; init; }
    public TimeOnly End { get; init; }
    public string Discipline { get; init; }
    public string StudyType { get; init; }
    public string Cabinet { get; init; }
    public string StudyGroup { get; init; }
    public List<EventItem>? Meetings { get; set; }
    public List<OutageItem> Outages { get; init; }
}