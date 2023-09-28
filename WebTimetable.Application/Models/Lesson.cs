namespace WebTimetable.Application.Models
{
    public class Lesson
    {
        public Guid Id { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
        public string Discipline { get; set; }
        public string StudyType { get; set; }
        public string Cabinet { get; set; }
        public string Teacher { get; set; }
        public string Subgroup { get; set; }
        public List<Outage> Outages { get; set; } = new();
    }
}
