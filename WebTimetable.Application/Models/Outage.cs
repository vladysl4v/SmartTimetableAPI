namespace WebTimetable.Application.Models
{
    public class Outage
    {
        public bool? IsDefinite { get; set; }
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
    }
}
