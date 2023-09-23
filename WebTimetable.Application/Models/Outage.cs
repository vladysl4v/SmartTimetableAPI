namespace WebTimetable.Application.Models
{
    public class Outage
    {
        public OutageType Type { get; set; }
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
        public string Text { get; set; }
    }
}
