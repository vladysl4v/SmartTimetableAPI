namespace WebTimetable.Contracts.DataTransferObjects;

public class TeacherLessonDTO
{
    public Guid Id { get; init; }
    public DateOnly Date { get; init; }
    public TimeOnly Start { get; init; }
    public TimeOnly End { get; init; }
    public string Discipline { get; init; }
    public string StudyType { get; init; }
    public string Cabinet { get; init; }
    public string StudyGroup { get; init; }
    public List<OutageDTO>? Outages { get; init; }
}